using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class UpdateShippingMarkCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ShippingMarkModel ShippingMark { get; set; }
    }

    public class UpdateShippingMarkCommandHandler : IRequestHandler<UpdateShippingMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public UpdateShippingMarkCommandHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var shippingMarkPrintings = new List<Entities.ShippingMarkPrinting>();

            var shippingMarkPrintingsPrinted = await _context.ShippingMarkPrintings
                .Where(i => i.ShippingMarkId == request.Id)
                .Where(x => x.Status.Equals(nameof(ShippingMarkStatus.Shipping)))
                .ToListAsync();

            var shippingMarkPrintingsGenerated = await _context.ShippingMarkPrintings
                .Where(i => i.ShippingMarkId == request.Id)
                .Where(x => x.Status.Equals(nameof(ShippingMarkStatus.New)))
                .ToListAsync();

            var groupByProducts = request.ShippingMark.ShippingMarkShippings
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ShippingMarkShippings = g.ToList(),
                    ShippingQuantity = request.ShippingMark.ReceivedMarkPrintings
                                        .Where(x => x.ProductId == g.Key)
                                        .ToList()
                                        .Sum(i => i.Quantity),
                });

            foreach (var group in groupByProducts)
            {
                int remainQty = group.ShippingQuantity;
                int printedQty = shippingMarkPrintingsPrinted.Where(x => x.ProductId == group.ProductId)
                                                             .ToList()
                                                             .Sum(i => i.Quantity);

                if (remainQty <= printedQty)
                {
                    return Result.Failure("Total Quantity can not be less than Printed Quantity");
                }

                _context.ShippingMarkPrintings.RemoveRange(shippingMarkPrintingsGenerated);
                
                remainQty -= printedQty;

                var product = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == group.ProductId);

                var lastItem = shippingMarkPrintingsPrinted
                    .Where(x => x.ProductId == group.ProductId)
                    .OrderBy(x => x.Sequence)
                    .LastOrDefault();

                int sequence = lastItem == null ? 1 : lastItem.Sequence + 1;

                while (remainQty > 0)
                {
                    if (remainQty >= product.QtyPerPackage)
                    {
                        shippingMarkPrintings.Add(new Entities.ShippingMarkPrinting
                        {
                            ProductId = product.Id,
                            Quantity = product.QtyPerPackage,
                            Sequence = sequence,
                            Status = nameof(ShippingMarkStatus.New),
                            ShippingMarkId = request.Id
                        });
                    }
                    else
                    {
                        shippingMarkPrintings.Add(new Entities.ShippingMarkPrinting
                        {
                            ProductId = product.Id,
                            Quantity = remainQty,
                            Sequence = sequence,
                            Status = nameof(ShippingMarkStatus.New),
                            ShippingMarkId = request.Id
                        });
                    }

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }

                var shippingMarkSummary = await _context.ShippingMarkSummaries
                    .FirstOrDefaultAsync(x => x.ProductId == group.ProductId && x.ShippingMarkId == request.Id);

                shippingMarkSummary.TotalPackage = sequence - 1;
                shippingMarkSummary.TotalQuantity = group.ShippingQuantity;
            }

            await _context.ShippingMarkPrintings.AddRangeAsync(shippingMarkPrintings);

            await AddOrUpdateReceivedMarkPrintings(request.Id, request.ShippingMark.ReceivedMarkPrintings);

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure("Failed to edit Shipping Mark. Please try again");
        }

        private async Task AddOrUpdateReceivedMarkPrintings(int shippingMarkId, ICollection<ReceivedMarkPrintingModel> receivedMarkPrintingModels)
        {
            var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                                    .Where(x => x.ShippingMarkId == shippingMarkId)
                                    .ToListAsync();

            var newReceivedMarkPrintings = receivedMarkPrintingModels
                .Where(i => !receivedMarkPrintings.Any(x => x.Id == i.Id))
                .ToList();


            foreach (var item in receivedMarkPrintings)
            {
                var model = receivedMarkPrintingModels.FirstOrDefault(x => x.Id == item.Id);

                if (model == null)
                {
                    var receivedMarkPrinting = await _context.ReceivedMarkPrintings.FindAsync(item.Id);
                    receivedMarkPrinting.ShippingMarkId = null;
                    receivedMarkPrinting.Status = nameof(ReceivedMarkStatus.Storage);
                }
            }

            foreach (var item in newReceivedMarkPrintings)
            {
                var receivedMarkPrinting = await _context.ReceivedMarkPrintings
                    .FirstOrDefaultAsync(x => x.Id == item.Id && x.ShippingMarkId == null && item.Status.Equals(nameof(ReceivedMarkStatus.Storage)));

                if (receivedMarkPrinting == null)
                {
                    throw new ArgumentNullException(nameof(receivedMarkPrinting));
                }

                receivedMarkPrinting.ShippingMarkId = shippingMarkId;
                receivedMarkPrinting.Status = nameof(ReceivedMarkStatus.Reserved);
            }
        }
    }
}

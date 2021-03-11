using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class UpdateReceivedMarkCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ReceivedMarkModel ReceivedMark { get; set; }
    }

    public class UpdateReceiveMarkCommandHandler : IRequestHandler<UpdateReceivedMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public UpdateReceiveMarkCommandHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            if (!request.ReceivedMark.ReceivedMarkMovements.Any())
            {
                return Result.Failure("Failed to update received mark");
            }

            var receivedMarkPrintings = new List<Entities.ReceivedMarkPrinting>();

            var receivedMarkPrintingsGenerated = await _context.ReceivedMarkPrintings
                .Where(i => i.ReceivedMarkId == request.Id)
                .Where(x => x.Status.Equals(nameof(ShippingMarkStatus.New)))
                .ToListAsync();

            var receivedMarkPrintingsPrinted = await _context.ReceivedMarkPrintings
               .Where(i => i.ReceivedMarkId == request.Id)
               .Where(x => !receivedMarkPrintingsGenerated.Contains(x))
               .ToListAsync();

            var groupByProducts = request.ReceivedMark.ReceivedMarkMovements
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ReceivedQty = g.ToList().Sum(i => i.Quantity)
                });

            foreach (var group in groupByProducts)
            {
                int remainQty = group.ReceivedQty;
                int printedQty = receivedMarkPrintingsPrinted.Where(x => x.ProductId == group.ProductId)
                                                             .ToList()
                                                             .Sum(i => i.Quantity);

                if (remainQty <= printedQty)
                {
                    return Result.Failure("Total Quantity can not be less than or equal Printed Quantity");
                }

                _context.ReceivedMarkPrintings.RemoveRange(receivedMarkPrintingsGenerated);
                remainQty -= printedQty;

                var product = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == group.ProductId);

                var lastItem = receivedMarkPrintingsPrinted
                    .Where(x => x.ProductId == group.ProductId)
                    .OrderBy(x => x.Sequence)
                    .LastOrDefault();

                int sequence = lastItem == null ? 1 : lastItem.Sequence + 1;

                while (remainQty > 0)
                {
                    if (remainQty >= product.QtyPerPackage)
                    {
                        receivedMarkPrintings.Add(new Entities.ReceivedMarkPrinting
                        {
                            ProductId = product.Id,
                            Quantity = product.QtyPerPackage,
                            Sequence = sequence,
                            Status = nameof(ReceivedMarkStatus.New),
                            ReceivedMarkId = request.Id
                        });
                    }
                    else
                    {
                        receivedMarkPrintings.Add(new Entities.ReceivedMarkPrinting
                        {
                            ProductId = product.Id,
                            Quantity = remainQty,
                            Sequence = sequence,
                            Status = nameof(ReceivedMarkStatus.New),
                            ReceivedMarkId = request.Id
                        });
                    }

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }

                var receivedMarkSummary = await _context.ReceivedMarkSummaries
                    .FirstOrDefaultAsync(x => x.ProductId == group.ProductId && x.ReceivedMarkId == request.Id);

                receivedMarkSummary.TotalPackage = sequence - 1;
                receivedMarkSummary.TotalQuantity = group.ReceivedQty;

                var receivedMarkMovements = await _context.ReceivedMarkMovements
                                                          .Where(i => i.ReceivedMarkId == request.Id && i.ProductId == group.ProductId)
                                                          .ToListAsync();

                foreach (var item in receivedMarkMovements)
                {
                    var model = request.ReceivedMark.ReceivedMarkMovements
                        .FirstOrDefault(i => i.ReceivedMarkId == item.ReceivedMarkId && i.ProductId == item.ProductId && i.MovementRequestId == item.MovementRequestId);

                    if (model == null)
                    {
                        _context.ReceivedMarkMovements.Remove(item);
                    }
                    else
                    {
                        item.Quantity = model.Quantity;
                    }
                }
            }

            await _context.ReceivedMarkPrintings.AddRangeAsync(receivedMarkPrintings);

            var receivedMark = await _context.ReceivedMarks
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync();

            receivedMark.Notes = request.ReceivedMark.Notes;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}

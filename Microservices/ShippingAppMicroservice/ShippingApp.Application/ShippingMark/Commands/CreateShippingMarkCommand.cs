using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class CreateShippingMarkCommand : IRequest<Result>
    {
        public ShippingMarkModel ShippingMark { get; set; }
    }

    public class CreateShippingMarkCommandHandler : IRequestHandler<CreateShippingMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public CreateShippingMarkCommandHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(CreateShippingMarkCommand request, CancellationToken cancellationToken)
        {
            if (request.ShippingMark.ShippingMarkShippings == null || !request.ShippingMark.ShippingMarkShippings.Any())
            {
                return Result.Failure("Failed to create shipping mark");
            }

            if (request.ShippingMark.ReceivedMarkPrintings == null || !request.ShippingMark.ReceivedMarkPrintings.Any())
            {
                return Result.Failure("Failed to create shipping mark");
            }

            var shippingMarkPrintings = new List<Entities.ShippingMarkPrinting>();
            var shippingMarkSummaries = new List<Entities.ShippingMarkSummary>();

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
                var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == group.ProductId);
                int sequence = 1;

                while (remainQty > 0)
                {
                    shippingMarkPrintings.Add(new Entities.ShippingMarkPrinting
                    {
                        ProductId = product.Id,
                        Quantity = remainQty >= product.QtyPerPackage ? product.QtyPerPackage : remainQty,
                        Sequence = sequence,
                        Status = nameof(ShippingMarkStatus.New),
                    });

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }

                shippingMarkSummaries.Add(new Entities.ShippingMarkSummary
                {
                    ProductId = group.ProductId,
                    TotalPackage = sequence - 1,
                    TotalQuantity = group.ShippingQuantity
                });
            }

            var shippingMark = new Entities.ShippingMark
            {
                Notes = request.ShippingMark.Notes,
                ShippingMarkPrintings = shippingMarkPrintings,
                ShippingMarkShippings = BuildShippingMarkShippings(request.ShippingMark.ShippingMarkShippings),
                ShippingMarkSummaries = shippingMarkSummaries
            };

            await _context.ShippingMarks.AddAsync(shippingMark);
            await _context.SaveChangesAsync();

            foreach (var item in request.ShippingMark.ReceivedMarkPrintings)
            {
                var receivedMarkPrinting = await _context.ReceivedMarkPrintings.FindAsync(item.Id);

                if (receivedMarkPrinting == null)
                {
                    throw new ArgumentNullException(nameof(receivedMarkPrinting));
                }

                receivedMarkPrinting.ShippingMarkId = shippingMark.Id;
                receivedMarkPrinting.Status = nameof(ReceivedMarkStatus.Reserved);
            }

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure("Failed to create shipping mark");
        }

        private List<Entities.ShippingMarkShipping> BuildShippingMarkShippings(ICollection<ShippingMarkShippingModel> shippingMarkShippings)
        {
            return shippingMarkShippings.Select(x => new Entities.ShippingMarkShipping
            {
                ShippingRequestId = x.ShippingRequestId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                ShippingMarkId = 0,
            }).ToList();
        }
    }
}

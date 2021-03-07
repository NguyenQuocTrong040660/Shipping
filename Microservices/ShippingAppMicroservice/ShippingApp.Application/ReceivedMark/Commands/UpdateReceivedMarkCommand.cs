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
            var receivedMarkSummaries = new List<Entities.ReceivedMarkSummary>();

            var groupByProducts = request.ReceivedMark.ReceivedMarkMovements
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ReceivedMarkMovements = g.ToList(),
                    ReceivedQty = g.ToList().Sum(i => i.Quantity)
                });

            foreach (var group in groupByProducts)
            {
                int remainQty = group.ReceivedQty;
                var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == group.ProductId);
                int sequence = 1;

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
                        });
                    }

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }

                receivedMarkSummaries.Add(new Entities.ReceivedMarkSummary
                {
                    ProductId = group.ProductId,
                    TotalPackage = sequence - 1,
                    TotalQuantity = group.ReceivedQty
                });
            }

            var receivedMark = new Entities.ReceivedMark
            {
                Notes = request.ReceivedMark.Notes,
                ReceivedMarkPrintings = receivedMarkPrintings,
                ReceivedMarkMovements = BuildReceivedMarkMovements(request.ReceivedMark.ReceivedMarkMovements),
                ReceivedMarkSummaries = receivedMarkSummaries
            };

            await _context.ReceivedMarks.AddAsync(receivedMark);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        private List<Entities.ReceivedMarkMovement> BuildReceivedMarkMovements(ICollection<ReceivedMarkMovementModel> receivedMarkMovements)
        {
            return receivedMarkMovements.Select(x => new Entities.ReceivedMarkMovement
            {
                MovementRequestId = x.MovementRequestId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                ReceivedMarkId = 0,
            }).ToList();
        }
    }
}

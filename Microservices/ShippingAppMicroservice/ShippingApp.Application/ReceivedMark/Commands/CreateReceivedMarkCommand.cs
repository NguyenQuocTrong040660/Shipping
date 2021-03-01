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
using ShippingApp.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class CreateReceivedMarkCommand : IRequest<Result>
    {
        public ReceivedMarkModel ReceivedMark { get; set; }
    }

    public class CreateReceiveMarkCommandHandler : IRequestHandler<CreateReceivedMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public CreateReceiveMarkCommandHandler(IMapper mapper,
            IShippingAppDbContext context,
            IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            if (request.ReceivedMark.ReceivedMarkMovements == null || !request.ReceivedMark.ReceivedMarkMovements.Any())
            {
                return Result.Failure("Failed to create received mark");
            }

            var receivedMarkPrintings = new List<Entities.ReceivedMarkPrinting>();
            var receivedMarkSummaries  = new List<Entities.ReceivedMarkSummary>();

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

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure("Failed to create received mark");
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

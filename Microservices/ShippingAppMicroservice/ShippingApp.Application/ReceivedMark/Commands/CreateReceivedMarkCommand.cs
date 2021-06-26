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
        private readonly IShippingAppDbContext _context;

        public CreateReceiveMarkCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(CreateReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            if (request.ReceivedMark.ReceivedMarkMovements == null || !request.ReceivedMark.ReceivedMarkMovements.Any())
            {
                return Result.Failure("Failed to create received mark");
            }

            var receivedMarkPrintings = new List<Entities.ReceivedMarkPrinting>();

            foreach (var receivedMarkMovement in request.ReceivedMark.ReceivedMarkMovements.OrderBy(x => x.WorkOrderId))
            {
                int remainQty = receivedMarkMovement.Quantity;
                var product = await _context.Products
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == receivedMarkMovement.ProductId, cancellationToken);
                int sequence = 1;

                while (remainQty > 0)
                {
                    receivedMarkPrintings.Add(new Entities.ReceivedMarkPrinting
                    {
                        ProductId = product.Id,
                        Quantity = remainQty >= product.QtyPerPackage ? product.QtyPerPackage : remainQty,
                        Sequence = sequence,
                        Status = nameof(ReceivedMarkStatus.New),
                        MovementRequestId = receivedMarkMovement.MovementRequestId,
                        WorkOrderId = receivedMarkMovement.WorkOrderId
                    });

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }
            }

            var receivedMark = new Entities.ReceivedMark
            {
                Notes = request.ReceivedMark.Notes,
                ReceivedMarkPrintings = receivedMarkPrintings,
                ReceivedMarkMovements = BuildReceivedMarkMovements(request.ReceivedMark.ReceivedMarkMovements),
            };

            await _context.ReceivedMarks.AddAsync(receivedMark);

            return await _context.SaveChangesAsync(cancellationToken) > 0
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
                WorkOrderId = x.WorkOrderId,
                ReceivedMarkId = 0,
            }).ToList();
        }
    }
}

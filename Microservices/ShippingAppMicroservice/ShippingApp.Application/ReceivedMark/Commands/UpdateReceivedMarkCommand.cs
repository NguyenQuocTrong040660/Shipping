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

            var receivedMarkPrintingsGenerated = await _context.ReceivedMarkPrintings
               .Where(i => i.ReceivedMarkId == request.Id)
               .Where(x => !x.Status.Equals(nameof(ShippingMarkStatus.New)))
               .ToListAsync(cancellationToken);

            if (receivedMarkPrintingsGenerated.Any())
            {
                return Result.Failure("Unable to edit Received Mark already printed mark");
            }

            var receivedMarkPrintings = new List<Entities.ReceivedMarkPrinting>();

            var receivedMark = await _context.ReceivedMarks
                .Include(x => x.ReceivedMarkMovements)
                .Include(x => x.ReceivedMarkPrintings)
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            receivedMark.ReceivedMarkMovements.Clear();
            receivedMark.ReceivedMarkPrintings.Clear();

            var groupByWorkOrder = request.ReceivedMark.ReceivedMarkMovements
                .OrderBy(x => x.WorkOrderId)
                .GroupBy(x => x.WorkOrderId)
                .Select(x => new
                {
                    WorkOrderId = x.Key,
                    ReceivedMarkMovements = x.ToList()
                })
                .ToList();

            foreach (var group in groupByWorkOrder)
            {
                int sequence = 1;

                foreach (var receivedMarkMovement in group.ReceivedMarkMovements)
                {
                    int remainQty = receivedMarkMovement.Quantity;
                    var product = await _context.Products
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == receivedMarkMovement.ProductId, cancellationToken);

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
            }

            receivedMark.ReceivedMarkPrintings = receivedMarkPrintings;
            receivedMark.ReceivedMarkMovements = BuildReceivedMarkMovements(request.ReceivedMark.ReceivedMarkMovements);
            receivedMark.Notes = request.ReceivedMark.Notes;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private List<Entities.ReceivedMarkMovement> BuildReceivedMarkMovements(ICollection<ReceivedMarkMovementModel> receivedMarkMovements)
        {
            return receivedMarkMovements.Select(x => new Entities.ReceivedMarkMovement
            {
                MovementRequestId = x.MovementRequestId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                WorkOrderId = x.WorkOrderId,
                ReceivedMarkId = x.ReceivedMarkId,
            }).ToList();
        }
    }
}

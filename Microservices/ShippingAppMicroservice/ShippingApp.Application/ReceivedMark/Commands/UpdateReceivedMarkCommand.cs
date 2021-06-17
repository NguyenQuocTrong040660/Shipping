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
                .ToListAsync(cancellationToken);

            var receivedMarkPrintingsPrinted = await _context.ReceivedMarkPrintings
               .Where(i => i.ReceivedMarkId == request.Id)
               .Where(x => !receivedMarkPrintingsGenerated.Contains(x))
               .ToListAsync(cancellationToken);
           
            foreach (var receivedMarkMovement in request.ReceivedMark.ReceivedMarkMovements)
            {
                int remainQty = receivedMarkMovement.Quantity;
                int printedQty = receivedMarkPrintingsPrinted.Where(x => x.ProductId == receivedMarkMovement.ProductId)
                                                             .Where(x => x.MovementRequestId == receivedMarkMovement.MovementRequestId)
                                                             .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.Storage)))
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
                    .FirstOrDefaultAsync(x => x.Id == receivedMarkMovement.ProductId, cancellationToken);

                var lastItem = receivedMarkPrintingsPrinted
                    .Where(x => x.ProductId == receivedMarkMovement.ProductId)
                    .OrderBy(x => x.Sequence)
                    .LastOrDefault();

                int sequence = lastItem == null ? 1 : lastItem.Sequence + 1;

                while (remainQty > 0)
                {
                    receivedMarkPrintings.Add(new Entities.ReceivedMarkPrinting
                    {
                        ProductId = product.Id,
                        Quantity = remainQty >= product.QtyPerPackage ? product.QtyPerPackage : remainQty,
                        Sequence = sequence,
                        Status = nameof(ReceivedMarkStatus.New),
                        ReceivedMarkId = request.Id
                    });

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }

                var receivedMarkMovements = await _context.ReceivedMarkMovements
                                                          .Where(i => i.ReceivedMarkId == request.Id)
                                                          .Where(i => i.ProductId == receivedMarkMovement.ProductId)
                                                          .ToListAsync(cancellationToken);

                foreach (var item in receivedMarkMovements)
                {
                    var model = request.ReceivedMark.ReceivedMarkMovements
                        .FirstOrDefault(i => i.ReceivedMarkId == item.ReceivedMarkId 
                                            && i.ProductId == item.ProductId 
                                            && i.MovementRequestId == item.MovementRequestId);

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
                .FirstOrDefaultAsync(cancellationToken);

            receivedMark.Notes = request.ReceivedMark.Notes;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

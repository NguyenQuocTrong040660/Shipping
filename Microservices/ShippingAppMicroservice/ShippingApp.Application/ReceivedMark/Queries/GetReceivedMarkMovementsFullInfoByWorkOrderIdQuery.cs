using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarkMovementsFullInfoByWorkOrderIdQuery : IRequest<List<ReceivedMarkMovementModel>>
    {
        public int WorkOrderId { get; set; }
    }
    public class GetReceivedMarkMovementsFullInfoByWorkOrderIdQueryHandler : IRequestHandler<GetReceivedMarkMovementsFullInfoByWorkOrderIdQuery, List<ReceivedMarkMovementModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetReceivedMarkMovementsFullInfoByWorkOrderIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ReceivedMarkMovementModel>> Handle(GetReceivedMarkMovementsFullInfoByWorkOrderIdQuery request, CancellationToken cancellationToken)
        {
            var receivedMarkMovements = _mapper.Map<List<ReceivedMarkMovementModel>>(await _context.ReceivedMarkMovements
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.WorkOrderId == request.WorkOrderId)
                .ToListAsync(cancellationToken));

            foreach (var item in receivedMarkMovements)
            {
                var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                  .AsNoTracking()
                  .Where(x => x.WorkOrderId == item.WorkOrderId)
                  .Where(x => x.MovementRequestId == item.MovementRequestId)
                  .Where(x => x.ReceivedMarkId == item.ReceivedMarkId)
                  .Where(x => !x.Status.Equals(nameof(ReceivedMarkStatus.Unstuff)))
                  .ToListAsync(cancellationToken);

                item.TotalPackage = receivedMarkPrintings.Count;

                item.TotalQuantityPrinted = receivedMarkPrintings
                    .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.Storage)))
                    .Sum(x => x.Quantity);
            }

            return receivedMarkMovements;
        }
    }
}

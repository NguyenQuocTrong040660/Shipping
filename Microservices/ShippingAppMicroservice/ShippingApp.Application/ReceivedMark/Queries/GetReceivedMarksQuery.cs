using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarksQuery : IRequest<List<ReceivedMarkModel>>
    {
    }

    public class GetReceiveMarksQueryHandler : IRequestHandler<GetReceivedMarksQuery, List<ReceivedMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;

        public GetReceiveMarksQueryHandler(IMapper mapper, IShippingAppDbContext context, IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ReceivedMarkModel>> Handle(GetReceivedMarksQuery request, CancellationToken cancellationToken)
        {
            var receivedMarks = _mapper.Map<List<ReceivedMarkModel>>(await _shippingAppRepository
                .GetDbSet()
                .AsNoTracking()
                .Include(x => x.ReceivedMarkMovements)
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken));

            foreach (var item in receivedMarks)
            {
                var movementWorkOrder = new List<string>();

                foreach (var receivedMarkMovement in item.ReceivedMarkMovements)
                {
                    var movementRequest = await _context.MovementRequests
                        .Include(x => x.MovementRequestDetails)
                        .ThenInclude(x => x.WorkOrder)
                        .FirstOrDefaultAsync(x => x.Id == receivedMarkMovement.MovementRequestId, cancellationToken);

                    receivedMarkMovement.WorkOrderMomentRequest = $"{movementRequest.Prefix}{movementRequest.Id}-{movementRequest.MovementRequestDetails.FirstOrDefault(x => x.ProductId == receivedMarkMovement.ProductId).WorkOrder.RefId}";
                }

                item.WorkOrdersMovementCollection = $"{string.Join(", ", item.ReceivedMarkMovements.Select(x => x.WorkOrderMomentRequest))} ";
            }

            return receivedMarks;
        }
    }
}

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

namespace ShippingApp.Application.MovementRequest.Queries
{
    public class GetMovementRequestsQuery : IRequest<List<MovementRequestModel>>
    {
    }

    public class GetMovementRequestsQueryHandler : IRequestHandler<GetMovementRequestsQuery, List<MovementRequestModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;

        public GetMovementRequestsQueryHandler(IMapper mapper, IShippingAppDbContext context, IShippingAppRepository<Entities.MovementRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<MovementRequestModel>> Handle(GetMovementRequestsQuery request, CancellationToken cancellationToken)
        {
            var movementRequests = _mapper.Map<List<MovementRequestModel>>(await _shippingAppRepository
                .GetDbSet()
                .AsNoTracking()
                .OrderByDescending(x => x.LastModified)
                .ToListAsync(cancellationToken));

            foreach (var item in movementRequests)
            {
                var movementDetails = await _context.MovementRequestDetails
                    .AsNoTracking()
                    .Include(x => x.WorkOrder)
                    .Where(x => x.MovementRequestId == item.Id)
                    .ToListAsync();

                item.WorkOrdersCollection = $"[{string.Join(",", movementDetails.Select(x => x.WorkOrder.RefId))}]";
                item.IsSelectedByReceivedMark = await _context.ReceivedMarkMovements.AnyAsync(x => x.MovementRequestId == item.Id);
            }
            
            return movementRequests;
        }
    }
}

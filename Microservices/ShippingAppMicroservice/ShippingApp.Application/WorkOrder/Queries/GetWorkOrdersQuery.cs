using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.WorkOrder.Queries
{
    public class GetWorkOrdersQuery : IRequest<List<WorkOrderModel>>
    {
    }

    public class GetWorkOrdersQueryHandler : IRequestHandler<GetWorkOrdersQuery, List<WorkOrderModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public GetWorkOrdersQueryHandler(IMapper mapper, IShippingAppRepository<Entities.WorkOrder> shippingAppRepository, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<WorkOrderModel>> Handle(GetWorkOrdersQuery request, CancellationToken cancellationToken)
        {
            var workOrders = await _context.WorkOrders
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            foreach (var item in workOrders)
            {
                item.WorkOrderDetails = await _context.WorkOrderDetails
                    .AsNoTracking()
                    .Include(x => x.Product)
                    .Where(x => x.WorkOrderId == item.Id)
                    .ToListAsync(cancellationToken);

                item.MovementRequestDetails = await _context.MovementRequestDetails
                    .AsNoTracking()
                    .Where(x => x.WorkOrderId == item.Id)
                    .ToListAsync(cancellationToken);
            }

            var result = _mapper.Map<List<WorkOrderModel>>(workOrders);

            foreach (var item in result)
            {
                item.ReceviedMarkQuantity = await PopulateReceviedMarkQuantityAsync(item, cancellationToken);
            }

            return result;
        }

        private async Task<int> PopulateReceviedMarkQuantityAsync(WorkOrderModel item, CancellationToken cancellationToken)
        {
            var receivedMarkMovements = await _context.ReceivedMarkMovements
                .Where(x => x.WorkOrderId == item.Id)
                .ToListAsync(cancellationToken);

            return receivedMarkMovements.Sum(x => x.Quantity);
        }
    }
}

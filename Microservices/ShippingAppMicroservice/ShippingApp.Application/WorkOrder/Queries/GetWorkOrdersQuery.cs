using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.WorkOrder.Queries
{
    public class GetWorkOrdersQuery : IRequest<List<WorkOrderModel>>
    {
    }

    public class GetWorkOrdersQueryHandler : IRequestHandler<GetWorkOrdersQuery, List<WorkOrderModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;

        public GetWorkOrdersQueryHandler(IMapper mapper, IShippingAppRepository<Entities.WorkOrder> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<WorkOrderModel>> Handle(GetWorkOrdersQuery request, CancellationToken cancellationToken)
        {
            var workOrders = await _shippingAppRepository.GetAllAsync();
            return _mapper.Map<List<WorkOrderModel>>(workOrders);
        }
    }
}

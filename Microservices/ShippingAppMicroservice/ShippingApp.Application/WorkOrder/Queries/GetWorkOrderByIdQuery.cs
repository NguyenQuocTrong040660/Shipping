using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.WorkOrder.Queries
{
    public class GetWorkOrderByIdQuery : IRequest<WorkOrderModel>
    {
        public int Id { get; set; }
    }
    public class GetWorkOrderByIdQueryHandler : IRequestHandler<GetWorkOrderByIdQuery, WorkOrderModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;

        public GetWorkOrderByIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.WorkOrder> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<WorkOrderModel> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var workOrder = await _shippingAppRepository
               .GetDbSet()
               .Include(i => i.WorkOrderDetails)
               .ThenInclude(i => i.Product)
               .OrderByDescending(i => i.LastModified)
               .FirstOrDefaultAsync(i => i.Id == request.Id);

            return _mapper.Map<WorkOrderModel>(workOrder);
        }
    }
}

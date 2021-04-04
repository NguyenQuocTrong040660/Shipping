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
        private readonly IShippingAppDbContext _context;
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;

        public GetWorkOrderByIdQueryHandler(IMapper mapper, IShippingAppDbContext context, IShippingAppRepository<Entities.WorkOrder> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<WorkOrderModel> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var workOrder = await _shippingAppRepository
               .GetDbSet()
               .Include(i => i.WorkOrderDetails).ThenInclude(i => i.Product)
               .Include(x => x.MovementRequestDetails).ThenInclude(x => x.Product)
               .OrderByDescending(i => i.LastModified)
               .FirstOrDefaultAsync(i => i.Id == request.Id);

            foreach (var item in workOrder.MovementRequestDetails)
            {
                item.MovementRequest = await _context.MovementRequests.FindAsync(item.MovementRequestId);
            }

            return _mapper.Map<WorkOrderModel>(workOrder);
        }
    }
}

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
using ShippingApp.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.WorkOrder.Commands
{
    public class CreateWorkOrderCommand : IRequest<Result>
    {
        public WorkOrderModel WorkOrder { get; set; }
    }

    public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public CreateWorkOrderCommandHandler(IMapper mapper, 
            IShippingAppRepository<Entities.WorkOrder> shippingAppRepository,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.WorkOrder>(request.WorkOrder);

            entity.WorkOrderDetails.ToList().ForEach(i => {
                i.Product = null;
            });

            entity.Status = nameof(WorkOrderStatus.Start);

            return await _shippingAppRepository.AddAsync(entity);
        }
    }
}

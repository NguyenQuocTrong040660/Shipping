using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.WorkOrder.Commands
{
    public class UpdateWorkOrderCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public WorkOrderModel WorkOrder { get; set; }
    }

    public class UpdateWorkOrderCommandHandler : IRequestHandler<UpdateWorkOrderCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;

        public UpdateWorkOrderCommandHandler(IMapper mapper, IShippingAppRepository<Entities.WorkOrder> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.WorkOrder>(request.WorkOrder);
            return await _shippingAppRepository.Update(request.Id, entity);
        }
    }
}

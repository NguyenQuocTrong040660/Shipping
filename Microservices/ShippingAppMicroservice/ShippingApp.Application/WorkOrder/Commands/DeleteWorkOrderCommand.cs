using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.WorkOrder.Commands
{
    public class DeleteWorkOrderCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteWorkOrderCommandHandler : IRequestHandler<DeleteWorkOrderCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;

        public DeleteWorkOrderCommandHandler(IShippingAppRepository<Entities.WorkOrder> shippingAppRepository)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(DeleteWorkOrderCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}

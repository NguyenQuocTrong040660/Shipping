using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;


namespace ShippingApp.Application.ShippingPlan.Commands
{
    public class DeleteShippingPlanCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }

    public class DeleteShippingPlanCommandHandler : IRequestHandler<DeleteShippingPlanCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.ShippingPlan> _shippingAppRepository;

        public DeleteShippingPlanCommandHandler(IShippingAppRepository<Entities.ShippingPlan> shippingAppRepository)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(DeleteShippingPlanCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}

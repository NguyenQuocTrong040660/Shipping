using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.ShippingPlan.Commands
{
    public class CreateNewShippingPLanCommand : IRequest<Result>
    {
        public ShippingPlanModel ShippingPlan { get; set; }
    }

    public class CreateNewShippingPLanCommandHandler : IRequestHandler<CreateNewShippingPLanCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingPlan> _shippingAppRepository;

        public CreateNewShippingPLanCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingPlan> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateNewShippingPLanCommand request, CancellationToken cancellationToken)
        {
            var shippingPlan = _mapper.Map<Entities.ShippingPlan>(request.ShippingPlan);
            return await _shippingAppRepository.AddAsync(shippingPlan);
        }
    }
}

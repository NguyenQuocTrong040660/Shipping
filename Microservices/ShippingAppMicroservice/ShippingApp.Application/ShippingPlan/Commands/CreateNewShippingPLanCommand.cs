using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Config.Queries;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ShippingPlan.Commands
{
    public class CreateNewShippingPLanCommand : IRequest<Result>
    {
        public ShippingPlanModel ShippingPlan { get; set; }
    }

    public class CreateNewShippingPLanCommandHandler : IRequestHandler<CreateNewShippingPLanCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IShippingAppRepository<Entities.ShippingPlan> _shippingAppRepository;

        public CreateNewShippingPLanCommandHandler(IMapper mapper, 
            IMediator mediator,
            IShippingAppRepository<Entities.ShippingPlan> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateNewShippingPLanCommand request, CancellationToken cancellationToken)
        {
            var config = await _mediator.Send(new GetConfigByKeyQuery
            {
                Key = ConfigKey.MinShippingDay
            });

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!int.TryParse(config.Value, out int numberDays))
            {
                throw new Exception("Failed to try parse value from config table");
            }


            if ((request.ShippingPlan.ShippingDate - DateTime.Now).TotalDays <= numberDays)
            {
                return Result.Failure($"Shipping Date should be larger than submit date {numberDays} days");
            }

            var shippingPlan = _mapper.Map<Entities.ShippingPlan>(request.ShippingPlan);
            return await _shippingAppRepository.AddAsync(shippingPlan);
        }
    }
}

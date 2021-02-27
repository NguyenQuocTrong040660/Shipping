using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using ShippingApp.Domain.Enumerations;
using ShippingApp.Application.Config.Queries;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class CreateShippingRequestCommand : IRequest<Result>
    {
        public ShippingRequestModel ShippingRequest { get; set; }
    }

    public class CreateShippingRequestCommandHandler : IRequestHandler<CreateShippingRequestCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;
        private readonly IMediator _mediator;

        public CreateShippingRequestCommandHandler(IMapper mapper,
            IMediator mediator,
            IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateShippingRequestCommand request, CancellationToken cancellationToken)
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

            if ((request.ShippingRequest.ShippingDate - DateTime.Now).TotalDays <= numberDays) 
            {
                return Result.Failure($"Shipping Date should be larger than submit date {numberDays} days");
            }

            var entity = _mapper.Map<Entities.ShippingRequest>(request.ShippingRequest);
            return await _shippingAppRepository.AddAsync(entity);
        }
    }
}

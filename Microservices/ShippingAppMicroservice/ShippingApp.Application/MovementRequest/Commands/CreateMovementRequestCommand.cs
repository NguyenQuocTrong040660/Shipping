using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.MovementRequest.Commands
{
    public class CreateMovementRequestCommand : IRequest<Result>
    {
        public MovementRequestModel MovementRequest { get; set; }
    }

    public class CreateMovementRequestCommandHandler : IRequestHandler<CreateMovementRequestCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;

        public CreateMovementRequestCommandHandler(IMapper mapper, IShippingAppRepository<Entities.MovementRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateMovementRequestCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Entities.MovementRequest>(request.MovementRequest);
            return await _shippingAppRepository.AddAsync(productEntity);
        }
    }
}

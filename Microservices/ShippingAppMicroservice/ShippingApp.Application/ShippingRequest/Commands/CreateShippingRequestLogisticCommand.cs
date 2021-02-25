using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class CreateShippingRequestLogisticCommand : IRequest<Result>
    {
        public int MovementRequestId { get; set; }
        public ShippingRequestLogisticModel ShippingRequestLogistic { get; set; }
    }

    public class CreateShippingRequestLogisticCommandHandler : IRequestHandler<CreateShippingRequestLogisticCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingRequestLogistic> _shippingAppRepository;

        public CreateShippingRequestLogisticCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingRequestLogistic> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateShippingRequestLogisticCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.ShippingRequestLogistic>(request.ShippingRequestLogistic);
            return await _shippingAppRepository.AddAsync(entity);
        }
    }
}

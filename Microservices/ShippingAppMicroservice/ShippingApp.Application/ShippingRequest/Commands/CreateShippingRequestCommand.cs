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
    public class CreateShippingRequestCommand : IRequest<Result>
    {
        public ShippingRequestModel ShippingRequest { get; set; }
    }

    public class CreateShippingRequestCommandHandler : IRequestHandler<CreateShippingRequestCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;

        public CreateShippingRequestCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateShippingRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.ShippingRequest>(request.ShippingRequest);
            return await _shippingAppRepository.AddAsync(entity);
        }
    }
}

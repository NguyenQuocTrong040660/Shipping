using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.ShippingRequest.Queries
{
    public class GetShippingRequestsQuery : IRequest<List<ShippingRequestModel>>
    {
    }

    public class GetShippingRequestsQueryHandler : IRequestHandler<GetShippingRequestsQuery, List<ShippingRequestModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;

        public GetShippingRequestsQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ShippingRequestModel>> Handle(GetShippingRequestsQuery request, CancellationToken cancellationToken)
        {
            var requests = await _shippingAppRepository.GetAllAsync();
            return _mapper.Map<List<ShippingRequestModel>>(requests);
        }
    }
}

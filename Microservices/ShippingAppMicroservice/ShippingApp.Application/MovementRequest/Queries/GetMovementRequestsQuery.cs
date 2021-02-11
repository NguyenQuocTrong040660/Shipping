using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.MovementRequest.Queries
{
    public class GetMovementRequestsQuery : IRequest<List<MovementRequestModel>>
    {
    }

    public class GetMovementRequestsQueryHandler : IRequestHandler<GetMovementRequestsQuery, List<MovementRequestModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;

        public GetMovementRequestsQueryHandler(IMapper mapper, IShippingAppRepository<Entities.MovementRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<MovementRequestModel>> Handle(GetMovementRequestsQuery request, CancellationToken cancellationToken)
        {
            var requests = await _shippingAppRepository.GetAllAsync();
            return _mapper.Map<List<MovementRequestModel>>(requests);
        }
    }
}

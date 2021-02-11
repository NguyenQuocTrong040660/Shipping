using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Config.Queries
{
    public class GetConfigsQuery : IRequest<List<ConfigModel>>
    {
    }

    public class GetConfigsQueryHandler : IRequestHandler<GetConfigsQuery, List<ConfigModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Config> _shippingAppRepository;

        public GetConfigsQueryHandler(IMapper mapper, IShippingAppRepository<Entities.Config> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ConfigModel>> Handle(GetConfigsQuery request, CancellationToken cancellationToken)
        {
            var requests = await _shippingAppRepository.GetAllAsync();
            return _mapper.Map<List<ConfigModel>>(requests);
        }
    }
}

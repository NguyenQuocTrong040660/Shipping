using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Config.Queries
{
    public class GetConfigByIdQuery : IRequest<ConfigModel>
    {
        public int Id { get; set; }
    }

    public class GetConfigByIdQueryHandler : IRequestHandler<GetConfigByIdQuery, ConfigModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Config> _shippingAppRepository;

        public GetConfigByIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.Config> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<ConfigModel> Handle(GetConfigByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _shippingAppRepository.GetAsync(request.Id);
            return _mapper.Map<ConfigModel>(entity);
        }
    }
}

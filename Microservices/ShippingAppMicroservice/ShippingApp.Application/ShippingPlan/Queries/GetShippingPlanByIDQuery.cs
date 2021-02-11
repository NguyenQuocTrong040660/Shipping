using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.ShippingPlan.Queries
{
    public class GetShippingPlanByIDQuery : IRequest<ShippingPlanModel>
    {
        public int Id { get; set; }
    }

    public class GetShippingPlanByIDQueryHandler : IRequestHandler<GetShippingPlanByIDQuery, ShippingPlanModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Product> _shippingAppRepository;

        public GetShippingPlanByIDQueryHandler(IMapper mapper, IShippingAppRepository<Entities.Product> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<ShippingPlanModel> Handle(GetShippingPlanByIDQuery request, CancellationToken cancellationToken)
        {
            var shippingPlan = await _shippingAppRepository.GetAsync(request.Id);
            return _mapper.Map<ShippingPlanModel>(shippingPlan);
        }
    }
}

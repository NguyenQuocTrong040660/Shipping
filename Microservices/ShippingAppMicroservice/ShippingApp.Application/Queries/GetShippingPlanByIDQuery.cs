using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;

using System.Linq;
using AutoMapper;

namespace ShippingApp.Application.Queries
{
    public class GetShippingPlanByIDQuery : IRequest<DTO.ShippingPlan>
    {
        public Guid Id;
    }

    public class GetShippingPlanByIDQueryHandler : IRequestHandler<GetShippingPlanByIDQuery, DTO.ShippingPlan>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;
        public GetShippingPlanByIDQueryHandler(IShippingAppRepository productRepository, IMapper mapper)
        {
            _shippingAppRepository = productRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DTO.ShippingPlan> Handle(GetShippingPlanByIDQuery request, CancellationToken cancellationToken)
        {
            var shippingPlan = await _shippingAppRepository.GetShippingPlanByID(request.Id);
            var results = _mapper.Map<DTO.ShippingPlan>(shippingPlan);

            return await Task.FromResult(results);
        }
    }
}

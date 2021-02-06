using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DTO = ShippingApp.Domain.DTO;
using Models = ShippingApp.Domain.Models;
using System.Linq;
using AutoMapper;

namespace ShippingApp.Application.Queries
{
    public class GetAllShippingPlanQuery : IRequest<List<DTO.ShippingPlan>>
    {
    }
    public class GetAllShippingPlanQueryHandler : IRequestHandler<GetAllShippingPlanQuery, List<DTO.ShippingPlan>>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public GetAllShippingPlanQueryHandler(IShippingAppRepository productRepository, IMapper mapper)
        {
            _shippingAppRepository = productRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<DTO.ShippingPlan>> Handle(GetAllShippingPlanQuery request, CancellationToken cancellationToken)
        {
            var shippingPlans = _shippingAppRepository.GetAllShippingPlan();

            var results = _mapper.Map<List<DTO.ShippingPlan>>(shippingPlans);

            return await Task.FromResult(results);
        }

    }

    
}

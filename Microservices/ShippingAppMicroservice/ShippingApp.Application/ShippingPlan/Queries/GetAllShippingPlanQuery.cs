using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ShippingPlan.Queries
{
    public class GetAllShippingPlanQuery : IRequest<List<ShippingPlanModel>>
    {
    }

    public class GetAllShippingPlanQueryHandler : IRequestHandler<GetAllShippingPlanQuery, List<ShippingPlanModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingPlan> _shippingAppRepository;

        public GetAllShippingPlanQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingPlan> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ShippingPlanModel>> Handle(GetAllShippingPlanQuery request, CancellationToken cancellationToken)
        {
            var shippingPlans = await _shippingAppRepository.GetDbSet()
                .Include(x => x.ShippingPlanDetails)
                .ToListAsync();

            return _mapper.Map<List<ShippingPlanModel>>(shippingPlans);
        }
    }
}

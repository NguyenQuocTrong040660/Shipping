using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.ShippingPlan.Queries
{
    public class GetShippingPlanByIDQuery : IRequest<ShippingPlanModel>
    {
        public int Id { get; set; }
    }

    public class GetShippingPlanByIDQueryHandler : IRequestHandler<GetShippingPlanByIDQuery, ShippingPlanModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingPlan> _shippingAppRepository;

        public GetShippingPlanByIDQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingPlan> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<ShippingPlanModel> Handle(GetShippingPlanByIDQuery request, CancellationToken cancellationToken)
        {
            var entity = await _shippingAppRepository.GetDbSet()
                .Include(x => x.ShippingPlanDetails)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return _mapper.Map<ShippingPlanModel>(entity);
        }
    }
}

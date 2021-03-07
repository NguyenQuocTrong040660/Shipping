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
        private readonly IShippingAppDbContext _context;

        public GetShippingPlanByIDQueryHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ShippingPlanModel> Handle(GetShippingPlanByIDQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingPlans
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            entity.ShippingPlanDetails = await _context.ShippingPlanDetails
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(i => i.ShippingPlanId == entity.Id)
                .ToListAsync();

            return _mapper.Map<ShippingPlanModel>(entity);
        }
    }
}

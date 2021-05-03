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
    public class GetShippingPlanByIdQuery : IRequest<ShippingPlanModel>
    {
        public int Id { get; set; }
    }

    public class GetShippingPlanByIdQueryHandler : IRequestHandler<GetShippingPlanByIdQuery, ShippingPlanModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetShippingPlanByIdQueryHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ShippingPlanModel> Handle(GetShippingPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingPlans
                .AsNoTracking()
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return _mapper.Map<ShippingPlanModel>(entity);
        }
    }
}

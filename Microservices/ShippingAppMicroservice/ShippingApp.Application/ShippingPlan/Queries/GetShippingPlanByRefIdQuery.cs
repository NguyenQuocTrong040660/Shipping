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
    public class GetShippingPlanByRefIdQuery : IRequest<ShippingPlanModel>
    {
        public string SalesOrder { get; set; }
        public string SalelineNumber { get; set; }
        public string ProductNumber { get; set; }
    }

    public class GetShippingPlanByRefIdQueryHandler : IRequestHandler<GetShippingPlanByRefIdQuery, ShippingPlanModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetShippingPlanByRefIdQueryHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ShippingPlanModel> Handle(GetShippingPlanByRefIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingPlans
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.SalelineNumber == request.SalelineNumber 
                                        && x.SalesOrder == request.SalesOrder 
                                        && x.Product.ProductNumber == request.ProductNumber, cancellationToken);

            return _mapper.Map<ShippingPlanModel>(entity);
        }
    }
}

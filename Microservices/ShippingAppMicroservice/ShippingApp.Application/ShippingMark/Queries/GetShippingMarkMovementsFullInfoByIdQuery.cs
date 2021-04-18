using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ShippingMark.Queries
{
    public class GetShippingMarkMovementsFullInfoByIdQuery : IRequest<List<ShippingMarkShippingModel>>
    {
        public int ShippingMarkId { get; set; }
    }
    public class GetShippingMarkMovementsFullInfoByIdQueryHandler : IRequestHandler<GetShippingMarkMovementsFullInfoByIdQuery, List<ShippingMarkShippingModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetShippingMarkMovementsFullInfoByIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ShippingMarkShippingModel>> Handle(GetShippingMarkMovementsFullInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var shippingMarkShippings = _mapper.Map<List<ShippingMarkShippingModel>>(await _context.ShippingMarkShippings
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.ShippingMarkId == request.ShippingMarkId)
                .ToListAsync());

            foreach (var item in shippingMarkShippings)
            {
                var shippingMarkPrintings = await _context.ShippingMarkPrintings
                  .AsNoTracking()
                  .Where(x => x.ProductId == item.ProductId)
                  .Where(x => x.ShippingMarkId == item.ShippingMarkId)
                  .ToListAsync();

                item.TotalQuantity = shippingMarkPrintings.Sum(x => x.Quantity);
                item.TotalPackage = shippingMarkPrintings.Count;
                item.TotalQuantityPrinted = shippingMarkPrintings
                    .Where(x => x.Status.Equals(nameof(ShippingMarkStatus.Shipping)))
                    .Sum(x => x.Quantity);
            }

            return shippingMarkShippings;
        }
    }
}

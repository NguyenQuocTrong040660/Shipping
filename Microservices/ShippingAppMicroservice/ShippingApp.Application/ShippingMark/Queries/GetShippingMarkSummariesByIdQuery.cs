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

namespace ShippingApp.Application.ShippingMark.Queries
{
    public class GetShippingMarkSummariesByIdQuery : IRequest<List<ShippingMarkSummaryModel>>
    {
        public int ShippingMarkId { get; set; }
    }
    public class GetShippingMarkSummariesByIdQueryHandler : IRequestHandler<GetShippingMarkSummariesByIdQuery, List<ShippingMarkSummaryModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetShippingMarkSummariesByIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ShippingMarkSummaryModel>> Handle(GetShippingMarkSummariesByIdQuery request, CancellationToken cancellationToken)
        {
            var shippingMarkSummaries = await _context.ShippingMarkSummaries
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.ShippingMarkId == request.ShippingMarkId)
                .ToListAsync();

            foreach (var item in shippingMarkSummaries)
            {
                item.Product.ShippingMarkPrintings = await _context.ShippingMarkPrintings
                  .AsNoTracking()
                  .Where(x => x.ProductId == item.ProductId && x.ShippingMarkId == request.ShippingMarkId)
                  .ToListAsync();
            }

            return _mapper.Map<List<ShippingMarkSummaryModel>>(shippingMarkSummaries);
        }
    }
}

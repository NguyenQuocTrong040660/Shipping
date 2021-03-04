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
    public class GetShippingPrintingsByIdQuery : IRequest<List<ShippingMarkPrintingModel>>
    {
        public int ShippingMarkId { get; set; }
        public int ProductId { get; set; }
    }

    public class GetShippingPrintingsByIdQueryHandler : IRequestHandler<GetShippingPrintingsByIdQuery, List<ShippingMarkPrintingModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetShippingPrintingsByIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ShippingMarkPrintingModel>> Handle(GetShippingPrintingsByIdQuery request, CancellationToken cancellationToken)
        {
            var shippingMarkPrintings = await _context.ShippingMarkPrintings
                .AsNoTracking()
                .Where(x => x.ShippingMarkId == request.ShippingMarkId && x.ProductId == request.ProductId)
                .Where(x => !x.Status.Equals(nameof(ShippingMarkStatus.Shipping)))
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            return _mapper.Map<List<ShippingMarkPrintingModel>>(shippingMarkPrintings);
        }
    }
}

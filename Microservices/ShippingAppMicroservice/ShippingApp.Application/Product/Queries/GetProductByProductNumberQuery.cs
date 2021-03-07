using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.Product.Queries
{
    public class GetProductByProductNumberQuery : IRequest<ProductModel>
    {
        public string ProductNumber { get; set; }
    }
    public class GetProductByProductNumberQueryHandler : IRequestHandler<GetProductByProductNumberQuery, ProductModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetProductByProductNumberQueryHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ProductModel> Handle(GetProductByProductNumberQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductNumber.Equals(request.ProductNumber));
            return _mapper.Map<ProductModel>(product);
        }
    }
}

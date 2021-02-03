using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Domain.Models;
using System.Linq;

namespace ShippingApp.Application.Queries
{
    public class GetProductQuery : IRequest<List<ProductOverview>>
    {
        public int companyIndex;
    }
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, List<ProductOverview>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductOverview>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            List<ProductOverview> productOverviews =  _productRepository.GetAllProducts(request.companyIndex);
            return  await Task.FromResult(productOverviews);
        }

    }

    
}

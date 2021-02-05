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
    public class GetProductHightLightQuery : IRequest<List<ProductOverview>>
    {
    }
    public class GetProductHightLightQueryHandler : IRequestHandler<GetProductHightLightQuery, List<ProductOverview>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductHightLightQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductOverview>> Handle(GetProductHightLightQuery request, CancellationToken cancellationToken)
        {
            //List<ProductOverview> productOverviews =  _productRepository.GetAllProductsHightLight();
            //return  await Task.FromResult(productOverviews);
            return null;
        }

    }

    
}

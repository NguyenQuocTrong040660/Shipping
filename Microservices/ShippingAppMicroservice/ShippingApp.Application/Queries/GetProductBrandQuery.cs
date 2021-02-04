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
    public class GetProductBrandQuery : IRequest<List<Brand>>
    {

    }
    public class GetProductBrandQueryHandler : IRequestHandler<GetProductBrandQuery, List<Brand>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductBrandQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

      
        public async Task<List<Brand>> Handle(GetProductBrandQuery request, CancellationToken cancellationToken)
        {
            //List<Brand> productBrand = _productRepository.GetProductBrand();
            //return await Task.FromResult(productBrand);
            return null;
        }
    }


}


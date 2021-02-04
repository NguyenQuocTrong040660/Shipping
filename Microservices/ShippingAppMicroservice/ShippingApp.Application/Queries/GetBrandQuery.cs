using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Queries
{
    public class GetBrandQuery : IRequest<List<Brand>>
    {
        public int companyIndex;
    }
    public class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, List<Brand>>
    {
        private readonly IProductRepository _productRepository;

        public GetBrandQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Brand>> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            //List<Brand> brands = _productRepository.GetAllBrand(request.companyIndex);
            //return await Task.FromResult(brands);
            return null;
        }
    }
}

using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Queries
{
    public class GetProductTypeQuery : IRequest<List<ProductType>>
    {
        public int companyIndex;

    }
    public class GetProductTypeQueryHandler : IRequestHandler<GetProductTypeQuery, List<ProductType>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductTypeQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<List<ProductType>> Handle(GetProductTypeQuery request, CancellationToken cancellationToken)
        {
            //List<ProductType> productTypes = _productRepository.GetAllProductType(request.companyIndex);
            //return await Task.FromResult(productTypes);
            return null;

        }
    }
}

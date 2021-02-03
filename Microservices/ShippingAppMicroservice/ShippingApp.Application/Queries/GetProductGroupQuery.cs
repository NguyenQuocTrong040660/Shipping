using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Queries
{
    public class GetProductGroupQuery: IRequest<ProductGroup>
    {
    }

    public class GetProductGroupQueryHandler : IRequestHandler<GetProductGroupQuery, ProductGroup>
    {
        private readonly IProductRepository _productRepository;
        
        public GetProductGroupQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductGroup> Handle(GetProductGroupQuery request, CancellationToken cancellationToken)
        {
            ProductGroup productGroups = _productRepository.GetAllProductGroup();
            return await Task.FromResult(productGroups);
        }

    }
}

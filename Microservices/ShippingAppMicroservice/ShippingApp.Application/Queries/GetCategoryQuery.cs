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
    public class GetCategoryQuery : IRequest<List<ProductType>>
    {

    }
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, List<ProductType>>
    {
        private readonly IProductRepository _productRepository;
        public GetCategoryQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        //public async Task<List<ProductType>> Handle(GetProductTypeQuery request, CancellationToken cancellationToken)
        //{
        //    List<ProductType> productTypes = _productRepository.GetAllProductType();
        //    return await Task.FromResult(productTypes);

        //}

        public async Task<List<ProductType>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            //List<ProductType> categories = _productRepository.GetAllCategory();
            //    return await Task.FromResult(categories);
            return null;
        }
    }
}

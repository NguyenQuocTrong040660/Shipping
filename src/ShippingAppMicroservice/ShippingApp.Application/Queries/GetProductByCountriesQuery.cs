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
    public class GetProductByCountriesQuery : IRequest<List<ProductOverview>>
    {
        public GetProductByCountriesQuery(string countryCode)
        {
            CountryCode = countryCode;
        }

        public string CountryCode { get; }
    }
    public class GetProductByCountriesQueryHandler : IRequestHandler<GetProductByCountriesQuery, List<ProductOverview>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductByCountriesQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductOverview>> Handle(GetProductByCountriesQuery request, CancellationToken cancellationToken)
        {
            List<ProductOverview> productOverviews = _productRepository.GetProductByCountries(request.CountryCode);
            return await Task.FromResult(productOverviews);
        }

    }

}

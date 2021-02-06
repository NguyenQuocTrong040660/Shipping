using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Queries
{
    public class GetCountryQuery : IRequest<List<Country>>
    {
    }
    public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, List<Country>>
    {
        private readonly IShippingAppRepository _shippingAppRepository;

        public GetCountryQueryHandler(IShippingAppRepository productRepository)
        {
            _shippingAppRepository = productRepository;
        }
    
        public async Task<List<Country>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            //List<Country> countries = _shippingAppRepository.GetAllCountry();
            //return await Task.FromResult(countries);
            return null;
        }
    }
}

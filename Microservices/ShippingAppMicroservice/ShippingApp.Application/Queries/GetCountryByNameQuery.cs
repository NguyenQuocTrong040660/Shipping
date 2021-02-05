using AutoMapper;
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
    public class GetCountryByNameQuery : IRequest<Country>
    {
        public string CountryName;
    }

    public class GetCountryByNameQueryHandler : IRequestHandler<GetCountryByNameQuery, Country>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public GetCountryByNameQueryHandler(IShippingAppRepository repository, IMapper mapper)
        {
            _shippingAppRepository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Country> Handle(GetCountryByNameQuery request, CancellationToken cancellationToken)
        {
            //var result = await _shippingAppRepository.GetCountryByName(request.CountryName);
            //return await Task.FromResult(_mapper.Map<Country>(result));
            return null;
        }
    }
}

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
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetCountryByNameQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Country> Handle(GetCountryByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetCountryByName(request.CountryName);
            return await Task.FromResult(_mapper.Map<Country>(result));
        }
    }
}

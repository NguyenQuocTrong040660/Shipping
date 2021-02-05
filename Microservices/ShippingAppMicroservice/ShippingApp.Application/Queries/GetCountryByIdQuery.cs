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
    public class GetCountryByIdQuery : IRequest<Country>
    {
        public string CountryCode;
    }

    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Country>
    {
        private readonly IShippingAppRepository _repository;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(IShippingAppRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Country> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            //var result = await _repository.GetCountryByCode(request.CountryCode);
            //return await Task.FromResult(_mapper.Map<Country>(result));
            return null;
        }
    }
}

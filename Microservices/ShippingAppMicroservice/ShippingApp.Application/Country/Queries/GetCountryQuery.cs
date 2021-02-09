using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Country.Queries
{
    public class GetCountryQuery : IRequest<List<CountryModel>>
    {
    }
    public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, List<CountryModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetCountryQueryHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<CountryModel>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            var countries = await _context.Countries.AsNoTracking().ToListAsync();
            return _mapper.Map<List<CountryModel>>(countries);
        }
    }
}

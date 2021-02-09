using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Country.Queries
{
    public class GetCountryByNameQuery : IRequest<CountryModel>
    {
        public string CountryName;
    }

    public class GetCountryByNameQueryHandler : IRequestHandler<GetCountryByNameQuery, CountryModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetCountryByNameQueryHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CountryModel> Handle(GetCountryByNameQuery request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(i => i.CountryName.ToLower()
                                                .Equals(request.CountryName.ToLower()));

            return _mapper.Map<CountryModel>(country);
        }
    }
}

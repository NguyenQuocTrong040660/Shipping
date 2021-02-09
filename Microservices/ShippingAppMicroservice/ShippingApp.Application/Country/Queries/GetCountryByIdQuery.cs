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
    public class GetCountryByIdQuery : IRequest<CountryModel>
    {
        public string CountryCode;
    }

    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetCountryByIdQueryHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CountryModel> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(i => i.CountryCode.Equals(request.CountryCode));
            return _mapper.Map<CountryModel>(country);
        }
    }
}

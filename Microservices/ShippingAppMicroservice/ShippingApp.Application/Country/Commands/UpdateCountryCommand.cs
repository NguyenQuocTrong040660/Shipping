using MediatR;
using System;
using ShippingApp.Domain.Models;
using System.Threading.Tasks;
using System.Threading;
using ShippingApp.Application.Interfaces;
using AutoMapper;
using ShippingApp.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.Country.Commands
{
    public class UpdateCountryCommand : IRequest<Result>
    {
        public CountryModel Entity { get; set; }
        public string CountryCode { get; set; }
    }

    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public UpdateCountryCommandHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Countries.FirstOrDefaultAsync(i => i.CountryCode.Equals(request.CountryCode));

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _mapper.Map(request.Entity, entity);

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Faild to update new country");
        }
    }
}

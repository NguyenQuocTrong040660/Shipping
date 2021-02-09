using MediatR;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Country.Commands
{
    public class DeleteCountryCommand : IRequest<Result>
    {
        public string CountryCode { get; set; }
    }
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteCountryCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(i => i.CountryCode.Equals(request.CountryCode));

            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            _context.Countries.Remove(country);

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Faild to delete country");
        }
    }
}





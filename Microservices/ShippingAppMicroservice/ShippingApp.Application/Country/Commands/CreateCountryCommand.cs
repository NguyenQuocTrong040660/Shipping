using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.Country.Commands
{
    public class CreateCountryCommand : IRequest<Result>
    {
        public CountryModel Country { get; set; }
    }
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public CreateCountryCommandHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Entities.Country>(request.Country);
            await _context.Countries.AddAsync(productEntity);

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Faild to create new country");
        }
    }
}
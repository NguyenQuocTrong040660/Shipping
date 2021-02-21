using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.Product.Commands
{
    public class CreateProductCommand : IRequest<Result>
    {
        public ProductModel Product { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Product> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public CreateProductCommandHandler(IMapper mapper,
              IShippingAppDbContext context,
            IShippingAppRepository<Entities.Product> shippingAppRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entityInDb = await _context.Products.FirstOrDefaultAsync(i => i.ProductNumber.Equals(request.Product.ProductNumber));

            if (entityInDb != null)
            {
                return Result.Failure("Product already existed in database");
            }

            var productEntity = _mapper.Map<Entities.Product>(request.Product);
            return await _shippingAppRepository.AddAsync(productEntity);
        }
    }
}

using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Product.Commands
{
    public class UpdateProductCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ProductModel Product { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Product> _shippingAppRepository;

        public UpdateProductCommandHandler(IMapper mapper, IShippingAppRepository<Entities.Product> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Entities.Product>(request.Product);
            return await _shippingAppRepository.Update(request.Id, productEntity);
        }
    }
}

using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Product.Queries
{
    public class GetAllProductQuery : IRequest<List<ProductModel>>
    {
    }

    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<ProductModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Product> _shippingAppRepository;

        public GetAllProductQueryHandler(IMapper mapper, IShippingAppRepository<Entities.Product> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ProductModel>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = await _shippingAppRepository.GetAllAsync();
            return _mapper.Map<List<ProductModel>>(products);
        }
    }
}

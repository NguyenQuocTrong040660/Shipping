using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Product.Queries
{
    public class GetProductByIdQuery : IRequest<ProductModel>
    {
        public int Id;
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Product> _shippingAppRepository;

        public GetProductByIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.Product> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<ProductModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _shippingAppRepository.GetAsync(request.Id);
            return _mapper.Map<ProductModel>(product);
        }
    }
}

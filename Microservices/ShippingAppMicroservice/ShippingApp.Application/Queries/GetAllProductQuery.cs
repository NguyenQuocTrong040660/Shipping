using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DTO = ShippingApp.Domain.DTO;
using Models = ShippingApp.Domain.Models;
using System.Linq;
using AutoMapper;

namespace ShippingApp.Application.Queries
{
    public class GetAllProductQuery : IRequest<List<DTO.Product>>
    {
    }
    public class GetProductQueryHandler : IRequestHandler<GetAllProductQuery, List<DTO.Product>>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IShippingAppRepository productRepository, IMapper mapper)
        {
            _shippingAppRepository = productRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<DTO.Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = _shippingAppRepository.GetAllProducts();

            var results = _mapper.Map<List<DTO.Product>>(products);

            return await Task.FromResult(results);
        }

    }

    
}

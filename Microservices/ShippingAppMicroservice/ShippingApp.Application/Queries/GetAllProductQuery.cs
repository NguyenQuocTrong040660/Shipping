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
        private readonly IShippingAppRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IShippingAppRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<DTO.Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var productOverviews = _productRepository.GetAllProducts();

            var results = _mapper.Map<List<DTO.Product>>(productOverviews);

            return await Task.FromResult(results);
        }

    }

    
}

using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;

using System.Linq;
using AutoMapper;

namespace ShippingApp.Application.Queries
{
    public class GetProductByIDQuery : IRequest<DTO.ProductDTO>
    {
        public Guid Id;
    }
    public class GetProductByIDQueryHandler : IRequestHandler<GetProductByIDQuery, DTO.ProductDTO>
    {
        private readonly IShippingAppRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductByIDQueryHandler(IShippingAppRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DTO.ProductDTO> Handle(GetProductByIDQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductsbyID(request.Id);
            var results = _mapper.Map<DTO.ProductDTO>(product);

            return await Task.FromResult(results);
        }
    }
}

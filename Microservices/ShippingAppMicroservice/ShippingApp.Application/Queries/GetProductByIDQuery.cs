using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Domain.Models;
using System.Linq;
using AutoMapper;

namespace ShippingApp.Application.Queries
{
    public class GetProductByIDQuery : IRequest<ProductOverview>
    {
        public Guid Id;
    }
    public class GetProductByIDQueryHandler : IRequestHandler<GetProductByIDQuery, ProductOverview>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductByIDQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductOverview> Handle(GetProductByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetProductsbyID(request.Id);
            return await Task.FromResult(_mapper.Map<ProductOverview>(result));
        }
    }
}

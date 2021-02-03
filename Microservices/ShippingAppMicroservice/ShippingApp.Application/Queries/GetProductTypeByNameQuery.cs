using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Queries
{
    public class GetProductTypeByNameQuery : IRequest<ProductType>
    {
        public string ProductTypeName;
    }

    public class GetProductTypeByNameQueryHandler : IRequestHandler<GetProductTypeByNameQuery,ProductType>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductTypeByNameQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductType> Handle(GetProductTypeByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetProductTypeByName(request.ProductTypeName);
            return await Task.FromResult(_mapper.Map<ProductType>(result));
        }
    }
}

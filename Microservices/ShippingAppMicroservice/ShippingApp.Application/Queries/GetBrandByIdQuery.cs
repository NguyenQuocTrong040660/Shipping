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
    public class GetBrandByIdQuery : IRequest<Brand>
    {
        public Guid BrandCode;
    }

    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Brand>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetBrandByIdQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Brand> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            //var result = await _repository.GetBrandByCode(request.BrandCode);
            //return await Task.FromResult(_mapper.Map<Brand>(result));
            return null;
        }
    }
}

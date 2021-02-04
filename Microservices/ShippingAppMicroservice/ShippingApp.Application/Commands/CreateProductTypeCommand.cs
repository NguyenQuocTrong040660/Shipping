using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Commands
{
    public class CreateProductTypeCommand : IRequest<int>
    {
        public ProductType Model { get; set; }
    }

    public class CreateProductTypeCommandHandler : IRequestHandler<CreateProductTypeCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public CreateProductTypeCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
        {
            //var entity = _mapper.Map<ProductType>(request.Model);

            //return await _repository.CreateProductType(entity);
            return 0;
        }
    }

}

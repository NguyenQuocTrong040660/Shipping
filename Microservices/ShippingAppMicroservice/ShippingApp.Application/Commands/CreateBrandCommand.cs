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
    public class CreateBrandCommand : IRequest<int>
    {
        public Brand Model { get; set; }
    }
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public CreateBrandCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            //var entity = _mapper.Map<Brand>(request.Model);

            //return await _repository.CreateBrand(entity);
            return 0;
        }
    }
}
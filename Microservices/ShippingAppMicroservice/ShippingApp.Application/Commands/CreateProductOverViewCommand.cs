using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Commands
{
    public class CreateProductOverViewCommand : IRequest<int>
    {
        public ProductOverview Model { get; set; }
    }
    public class CreateProductOverViewCommandHandler : IRequestHandler<CreateProductOverViewCommand, int>
    {
        private readonly IProductRepository _Repository;
        private readonly IMapper _mapper;

        public CreateProductOverViewCommandHandler(IProductRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateProductOverViewCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ProductOverview>(request.Model);

            return await _Repository.CreateProductOverView(entity);
        }
    }
}

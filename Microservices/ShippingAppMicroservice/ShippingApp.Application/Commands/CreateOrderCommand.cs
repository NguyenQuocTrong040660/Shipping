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
    public class CreateOrderCommand : IRequest<int>
    {
        public Order Order { get; set; }
    }
    public class CreateOrderDetailCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;


        public CreateOrderDetailCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Order>(request.Order);

            return await _repository.CreatOder(entity);
        }
    }
}
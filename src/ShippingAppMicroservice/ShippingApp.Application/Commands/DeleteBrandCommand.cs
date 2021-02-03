using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Commands
{
    public class DeleteBrandCommand : IRequest<int>
    {   
        public Guid Id { get; set; }
    }
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public DeleteBrandCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteBrand(request.Id);
        }
    }

}





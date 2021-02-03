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
    public class DeleteProductOverViewCommand : IRequest<int>
    {
     public Guid Id { get; set; }
    }

    public class DeleteProductOverViewCommandHandler : IRequestHandler<DeleteProductOverViewCommand, int>
    {
        private readonly IProductRepository _Repository;
        private readonly IMapper _mapper;

        public DeleteProductOverViewCommandHandler(IProductRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeleteProductOverViewCommand request, CancellationToken cancellationToken)
        {
            return await _Repository.DeleteProductOverView(request.Id);
        }
    }
}

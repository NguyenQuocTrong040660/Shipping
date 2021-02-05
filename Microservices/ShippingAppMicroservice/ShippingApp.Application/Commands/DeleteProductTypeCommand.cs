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
    public class DeleteProductTypeCommand: IRequest<int>
    {
        public Guid ProductTypeCode { get; set; }
    }

    public class DeleteProductTypeCommandHandler : IRequestHandler<DeleteProductTypeCommand, int>
    {
        private readonly IShippingAppRepository _repository;
        private readonly IMapper _mapper;

        public DeleteProductTypeCommandHandler(IShippingAppRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
        {
            //return await _repository.DeleteProductType(request.ProductTypeCode);
            return 0;
        }
    }
}

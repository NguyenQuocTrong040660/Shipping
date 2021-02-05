using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using ShippingApp.Domain.Models;
using System.Threading.Tasks;
using System.Threading;
using ShippingApp.Domain.Interfaces;
using AutoMapper;
using System.Net.WebSockets;
using ShippingApp.Application.Common.Exceptions;

namespace ShippingApp.Application.Commands
{
    public class UpdateProductTypeCommand: IRequest<int>
    {
        public ProductType Entity { get; set; }
        public Guid ProductTypeCode { get; set; }
    }

    public class UpdateProductTypeCommanddHandler : IRequestHandler<UpdateProductTypeCommand, int>
    {
        private readonly IShippingAppRepository _repository;
        private readonly IMapper _mapper;

        public UpdateProductTypeCommanddHandler(IShippingAppRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<int> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
        {
            //var entity = await _repository.GetProductTypeByCode(request.ProductTypeCode);

            //if (entity == null)
            //{
            //    throw new NotFoundException(nameof(entity), request.ProductTypeCode);
            //}

            //return await _repository.UpdateProductType(_mapper.Map<ProductType>(request.Entity));
            return 0;
        }
    }
}

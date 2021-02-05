using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShippingApp.Application.Common.Exceptions;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;

namespace ShippingApp.Application.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public DTO.ProductDTO productDTO { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IShippingAppRepository shippingAppRepository, IMapper mapper)
        {
            _shippingAppRepository = shippingAppRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productModel = _mapper.Map<Models.ProductModel>(request.productDTO);

            return await _shippingAppRepository.UpdateProduct(productModel);
        }
    }


}

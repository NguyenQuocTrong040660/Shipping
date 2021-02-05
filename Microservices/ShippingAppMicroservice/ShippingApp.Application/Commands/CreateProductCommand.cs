﻿using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public DTO.Product productDTO { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IShippingAppRepository Repository, IMapper mapper)
        {
            _shippingAppRepository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productModel = _mapper.Map<Models.Product>(request.productDTO);

            return await _shippingAppRepository.CreateNewProduct(productModel);
        }
    }
}

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

namespace ShippingApp.Application.Commands
{
    public class UpdateProductOverViewCommand : IRequest<int>
    {
        public ProductOverview Entity { get; set; }
        public Guid Id { get; set; }
    }
    public class UpdateProductOverViewCommandHandler : IRequestHandler<UpdateProductOverViewCommand, int>
    {
        private readonly IProductRepository _Repository;
        private readonly IMapper _mapper;

        public UpdateProductOverViewCommandHandler(IProductRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(UpdateProductOverViewCommand request, CancellationToken cancellationToken)
        {
            //var entity = await _Repository.GetProductsbyID(request.Id);

            //if (entity == null)
            //{
            //    throw new NotFoundException(nameof(entity), request.Id);
            //}

            //return await _Repository.UpdateProductOverView(_mapper.Map<Entities.ProductOverview>(request.Entity));
            return 0;
        }
    }


}

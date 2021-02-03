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
    public class UpdateBrandCommand : IRequest<int>
    {
        public Brand Entity { get; set; }
        public Guid Id { get; set; }
    }

    public class UpdateBrandCommanddHandler : IRequestHandler<UpdateBrandCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public UpdateBrandCommanddHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<int> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetBrandByCode(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(entity), request.Id);
            }

            return await _repository.UpdateBrand(_mapper.Map<Brand>(request.Entity));
        }
    }
}

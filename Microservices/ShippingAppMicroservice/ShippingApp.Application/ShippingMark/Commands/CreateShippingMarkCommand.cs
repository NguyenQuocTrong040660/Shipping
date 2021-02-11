using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class CreateShippingMarkCommand : IRequest<Result>
    {
        public ShippingMarkModel ShippingMark { get; set; }
    }

    public class CreateShippingMarkCommandHandler : IRequestHandler<CreateShippingMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingMark> _shippingAppRepository;

        public CreateShippingMarkCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.ShippingMark>(request.ShippingMark);
            return await _shippingAppRepository.AddAsync(entity);
        }
    }
}

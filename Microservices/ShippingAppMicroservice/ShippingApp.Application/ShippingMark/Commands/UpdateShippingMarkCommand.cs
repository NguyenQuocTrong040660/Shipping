using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class UpdateShippingMarkCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ShippingMarkModel ShippingMark { get; set; }
    }

    public class UpdateShippingMarkCommandHandler : IRequestHandler<UpdateShippingMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingMark> _shippingAppRepository;

        public UpdateShippingMarkCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.ShippingMark>(request.ShippingMark);
            return await _shippingAppRepository.Update(request.Id, entity);
        }
    }
}

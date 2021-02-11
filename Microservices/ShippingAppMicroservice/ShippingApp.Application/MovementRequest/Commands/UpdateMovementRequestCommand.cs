using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.MovementRequest.Commands
{
    public class UpdateMovementRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public MovementRequestModel MovementRequest { get; set; }
    }

    public class UpdateMovementRequestCommandHandler : IRequestHandler<UpdateMovementRequestCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;

        public UpdateMovementRequestCommandHandler(IMapper mapper, IShippingAppRepository<Entities.MovementRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateMovementRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.MovementRequest>(request.MovementRequest);
            return await _shippingAppRepository.Update(request.Id, entity);
        }
    }
}

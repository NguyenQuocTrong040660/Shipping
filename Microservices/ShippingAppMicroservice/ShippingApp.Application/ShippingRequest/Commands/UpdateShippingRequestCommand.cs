using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class UpdateShippingRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ShippingRequestModel ShippingRequest { get; set; }
    }

    public class UpdateShippingRequestCommandHandler : IRequestHandler<UpdateShippingRequestCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;

        public UpdateShippingRequestCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateShippingRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.ShippingRequest>(request.ShippingRequest);
            return await _shippingAppRepository.Update(request.Id, entity);
        }
    }
}

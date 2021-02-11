using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class CreateReceivedMarkCommand : IRequest<Result>
    {
        public ReceivedMarkModel ReceivedMark { get; set; }
    }

    public class CreateReceiveMarkCommandHandler : IRequestHandler<CreateReceivedMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;

        public CreateReceiveMarkCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.ReceivedMark>(request.ReceivedMark);
            return await _shippingAppRepository.AddAsync(entity);
        }
    }
}

using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class UpdateReceivedMarkCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ReceivedMarkModel ReceivedMark { get; set; }
    }

    public class UpdateReceiveMarkCommandHandler : IRequestHandler<UpdateReceivedMarkCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;

        public UpdateReceiveMarkCommandHandler(IMapper mapper, IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.ReceivedMark>(request.ReceivedMark);
            return await _shippingAppRepository.Update(request.Id, entity);
        }
    }
}

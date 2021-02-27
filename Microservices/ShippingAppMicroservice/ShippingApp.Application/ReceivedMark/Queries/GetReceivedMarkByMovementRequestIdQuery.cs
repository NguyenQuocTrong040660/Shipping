using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ShippingApp.Application.ReceivedMark.Commands;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarkByMovementRequestIdQuery : IRequest<List<ReceivedMarkModel>>
    {
        public int MovementRequestId { get; set; }
    }

    public class GetReceivedMarkByMovementRequestIdQueryHandler : IRequestHandler<GetReceivedMarkByMovementRequestIdQuery, List<ReceivedMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;
        private readonly IMediator _mediator;

        public GetReceivedMarkByMovementRequestIdQueryHandler(IMapper mapper,
            IMediator mediator,
            IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ReceivedMarkModel>> Handle(GetReceivedMarkByMovementRequestIdQuery request, CancellationToken cancellationToken)
        {
            var receivedMarks = await _shippingAppRepository.GetDbSet()
                 .Include(x => x.Product)
                 .Where(x => x.MovementRequestId == request.MovementRequestId)
                 .ToListAsync();

            if (receivedMarks.Count == 0)
            {
                var generatedReceivedMarks = await _mediator.Send(new GenerateReceivedMarkCommand { MovementRequestId = request.MovementRequestId });
                return _mapper.Map<List<ReceivedMarkModel>>(generatedReceivedMarks);
            }

            return _mapper.Map<List<ReceivedMarkModel>>(receivedMarks);
        }
    }
}

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

        public GetReceivedMarkByMovementRequestIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ReceivedMarkModel>> Handle(GetReceivedMarkByMovementRequestIdQuery request, CancellationToken cancellationToken)
        {
            var receivedMarks = await _shippingAppRepository.GetDbSet()
                 .Include(x => x.Product)
                 .Where(x => x.MovementRequestId == request.MovementRequestId)
                 .ToListAsync();

            return _mapper.Map<List<ReceivedMarkModel>>(receivedMarks);
        }
    }
}

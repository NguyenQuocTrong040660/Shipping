using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarksQuery : IRequest<List<ReceivedMarkModel>>
    {
    }

    public class GetReceiveMarksQueryHandler : IRequestHandler<GetReceivedMarksQuery, List<ReceivedMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;

        public GetReceiveMarksQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ReceivedMarkModel>> Handle(GetReceivedMarksQuery request, CancellationToken cancellationToken)
        {
            var receivedMarks = await _shippingAppRepository.GetDbSet().ToListAsync();
            return _mapper.Map<List<ReceivedMarkModel>>(receivedMarks);
        }
    }
}

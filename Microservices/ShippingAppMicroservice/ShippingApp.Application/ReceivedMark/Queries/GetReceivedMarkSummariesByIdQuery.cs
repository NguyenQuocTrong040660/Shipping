using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarkSummariesByIdQuery : IRequest<List<ReceivedMarkSummaryModel>>
    {
        public int ReceivedMarkId { get; set; }
    }
    public class GetReceivedMarkSummariesByIdQueryHandler : IRequestHandler<GetReceivedMarkSummariesByIdQuery, List<ReceivedMarkSummaryModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetReceivedMarkSummariesByIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ReceivedMarkSummaryModel>> Handle(GetReceivedMarkSummariesByIdQuery request, CancellationToken cancellationToken)
        {
            var receivedMarkSummaries = await _context.ReceivedMarkSummaries
                .Include(x => x.Product)
                .AsNoTracking()
                .Where(x => x.ReceivedMarkId == request.ReceivedMarkId)
                .ToListAsync();

            return _mapper.Map<List<ReceivedMarkSummaryModel>>(receivedMarkSummaries);
        }
    }
}

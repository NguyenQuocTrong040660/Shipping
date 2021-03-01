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
    public class GetReceivedMarkPrintingsByIdQuery : IRequest<List<ReceivedMarkPrintingModel>>
    {
        public int ReceivedMarkId { get; set; }
        public int ProductId { get; set; }
    }

    public class GetReceivedMarkPrintingsByIdQueryHandler : IRequestHandler<GetReceivedMarkPrintingsByIdQuery, List<ReceivedMarkPrintingModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetReceivedMarkPrintingsByIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ReceivedMarkPrintingModel>> Handle(GetReceivedMarkPrintingsByIdQuery request, CancellationToken cancellationToken)
        {
            var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                .AsNoTracking()
                .Where(x => x.ReceivedMarkId == request.ReceivedMarkId && x.ProductId == request.ProductId)
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            return _mapper.Map<List<ReceivedMarkPrintingModel>>(receivedMarkPrintings);
        }
    }
}

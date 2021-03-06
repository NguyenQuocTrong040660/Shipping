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
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarkPrintingsByProductIdQuery : IRequest<List<ReceivedMarkPrintingModel>>
    {
        public int ProductId { get; set; }
    }

    public class GetReceivedMarkPrintingsByProductIdQueryHandler : IRequestHandler<GetReceivedMarkPrintingsByProductIdQuery, List<ReceivedMarkPrintingModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetReceivedMarkPrintingsByProductIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ReceivedMarkPrintingModel>> Handle(GetReceivedMarkPrintingsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                .AsNoTracking()
                .Where(x => x.ProductId == request.ProductId)
                .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.Storage)) 
                         || x.Status.Equals(nameof(ReceivedMarkStatus.Reserved)))
                .OrderBy(x => x.LastModified)
                .ToListAsync();

            return _mapper.Map<List<ReceivedMarkPrintingModel>>(receivedMarkPrintings);
        }
    }
}

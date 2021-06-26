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
using System.Linq;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarksGroupByWorkOrdersQuery : IRequest<List<WorkOrderModel>>
    {
    }

    public class GetReceivedMarksGroupByWorkOrdersQueryHandler : IRequestHandler<GetReceivedMarksGroupByWorkOrdersQuery, List<WorkOrderModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetReceivedMarksGroupByWorkOrdersQueryHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<WorkOrderModel>> Handle(GetReceivedMarksGroupByWorkOrdersQuery request, CancellationToken cancellationToken)
        {
            int[] workOrderIds = await _context.ReceivedMarkMovements.Select(x => x.WorkOrderId).Distinct().ToArrayAsync(cancellationToken);

            var workOrders = _mapper.Map<List<WorkOrderModel>>(await _context.WorkOrders
                .AsNoTracking()
                .Include(x => x.Product)
                .OrderBy(x => x.Id)
                .Where(x => workOrderIds.Contains(x.Id))
                .ToListAsync(cancellationToken));

            return workOrders;
        }
    }
}

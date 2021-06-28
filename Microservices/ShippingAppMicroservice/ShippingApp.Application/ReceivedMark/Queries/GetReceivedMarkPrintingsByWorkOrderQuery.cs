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
    public class GetReceivedMarkPrintingsByWorkOrderQuery : IRequest<List<ReceivedMarkPrintingModel>>
    {
        public int WorkOrderId { get; set; }
    }

    public class GetReceivedMarkPrintingsByWorkOrderQueryHandler : IRequestHandler<GetReceivedMarkPrintingsByWorkOrderQuery, List<ReceivedMarkPrintingModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetReceivedMarkPrintingsByWorkOrderQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ReceivedMarkPrintingModel>> Handle(GetReceivedMarkPrintingsByWorkOrderQuery request, CancellationToken cancellationToken)
        {
            var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                .AsNoTracking()
                .Where(x => x.WorkOrderId == request.WorkOrderId)
                .Where(x => !x.Status.Equals(nameof(ReceivedMarkStatus.Unstuff)))
                .OrderBy(x => x.Sequence)
                .ToListAsync(cancellationToken);

            var vm = _mapper.Map<List<ReceivedMarkPrintingModel>>(receivedMarkPrintings);

            foreach (var item in vm)
            {
                item.WorkOrder = _mapper.Map<WorkOrderModel>(await _context.WorkOrders.FindAsync(item.WorkOrderId));
            }

            return vm;
        }
    }
}

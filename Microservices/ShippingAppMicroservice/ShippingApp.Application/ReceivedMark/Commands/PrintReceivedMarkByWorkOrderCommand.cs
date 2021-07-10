using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Entities = ShippingApp.Domain.Entities;
using System.Linq;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class PrintReceivedMarkByWorkOrderCommand : IRequest<ReceivedMarkPrintingModel>
    {
        public int WorkOrderId { get; set; }
        public string PrintBy { get; set; }
    }

    public class PrintReceivedMarkByWorkOrderCommandHandler : IRequestHandler<PrintReceivedMarkByWorkOrderCommand, ReceivedMarkPrintingModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public PrintReceivedMarkByWorkOrderCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReceivedMarkPrintingModel> Handle(PrintReceivedMarkByWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                .Include(x => x.Product)
                .Include(x => x.ReceivedMark)
                .Where(x => x.WorkOrderId == request.WorkOrderId)
                .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.New)))
                .OrderBy(x => x.Sequence)
                .ToListAsync(cancellationToken);

            if (receivedMarkPrintings == null || !receivedMarkPrintings.Any())
            {
                return null;
            }

            Entities.ReceivedMarkPrinting printItem = null;

            for (int i = 0; i < receivedMarkPrintings.Count; i++)
            {
                var itemPrint = receivedMarkPrintings[i];

                if (itemPrint.PrintCount != 0)
                {
                    continue;
                }

                itemPrint.PrintCount += 1;
                itemPrint.Status = nameof(ReceivedMarkStatus.Storage);
                itemPrint.PrintingBy = request.PrintBy;
                itemPrint.PrintingDate = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                printItem = itemPrint;
                break;
            }

            var result = _mapper.Map<ReceivedMarkPrintingModel>(printItem);

            if (result != null)
            {
                result.WorkOrder = _mapper.Map<WorkOrderModel>(await _context.WorkOrders
                    .FirstOrDefaultAsync(x => x.Id == result.WorkOrderId, cancellationToken));
            }

            return result;
        }
    }
}

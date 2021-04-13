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
    public class PrintReceivedMarkCommand : IRequest<ReceivedMarkPrintingModel>
    {
        public PrintReceivedMarkRequest PrintReceivedMarkRequest { get; set; }
    }

    public class PrintReceivedMarkCommandHandler : IRequestHandler<PrintReceivedMarkCommand, ReceivedMarkPrintingModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public PrintReceivedMarkCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReceivedMarkPrintingModel> Handle(PrintReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                .Include(x => x.Product)
                .Include(x => x.ReceivedMark)
                .Where(x => x.ReceivedMarkId == request.PrintReceivedMarkRequest.ReceivedMarkId 
                         && x.ProductId == request.PrintReceivedMarkRequest.ProductId
                         && x.MovementRequestId == request.PrintReceivedMarkRequest.MovementRequestId)
                .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.New)))
                .OrderBy(x => x.Sequence)
                .ToListAsync();

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
                itemPrint.PrintingBy = request.PrintReceivedMarkRequest.PrintedBy;
                itemPrint.PrintingDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                printItem = itemPrint;
                break;
            }

            var result = _mapper.Map<ReceivedMarkPrintingModel>(printItem);

            if (result != null)
            {
                result.WorkOrder = _mapper.Map<WorkOrderModel>((await _context.WorkOrderDetails
                    .Include(x => x.WorkOrder)
                    .FirstOrDefaultAsync(x => x.ProductId == result.ProductId)).WorkOrder);
            }

            return result;
        }
    }
}

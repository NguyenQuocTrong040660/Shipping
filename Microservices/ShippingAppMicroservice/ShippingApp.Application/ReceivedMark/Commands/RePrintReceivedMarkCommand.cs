using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class RePrintReceivedMarkCommand : IRequest<ReceivedMarkPrintingModel>
    {
        public RePrintReceivedMarkRequest RePrintReceivedMarkRequest { get; set; }
    }

    public class RePrintReceivedMarkCommandHandler : IRequestHandler<RePrintReceivedMarkCommand, ReceivedMarkPrintingModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public RePrintReceivedMarkCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReceivedMarkPrintingModel> Handle(RePrintReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var receivedMarkPrinting = await _context.ReceivedMarkPrintings
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == request.RePrintReceivedMarkRequest.ReceivedMarkPrintingId, cancellationToken);

            if (receivedMarkPrinting == null)
            {
                return null;
            }

            receivedMarkPrinting.PrintCount += 1;
            receivedMarkPrinting.RePrintingBy = request.RePrintReceivedMarkRequest.RePrintedBy;
            receivedMarkPrinting.RePrintingDate = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync(cancellationToken) > 0
                    ? _mapper.Map<ReceivedMarkPrintingModel>(receivedMarkPrinting)
                    : null;

            if (result != null)
            {
                result.WorkOrder = _mapper.Map<WorkOrderModel>(await _context.WorkOrders
                    .FirstOrDefaultAsync(x => x.Id == result.WorkOrderId, cancellationToken));
            }

            return result;
        }
    }
}

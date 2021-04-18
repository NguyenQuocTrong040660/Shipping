using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class PrintShippingMarkCommand : IRequest<ShippingMarkPrintingModel>
    {
        public PrintShippingMarkRequest PrintShippingMarkRequest { get; set; }
    }

    public class PrintShippingMarkCommandHandler : IRequestHandler<PrintShippingMarkCommand, ShippingMarkPrintingModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public PrintShippingMarkCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShippingMarkPrintingModel> Handle(PrintShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var shippingMarkPrintings = await _context.ShippingMarkPrintings
                .Include(x => x.Product)
                .Include(x => x.ShippingMark)
                .Where(x => x.ShippingMarkId == request.PrintShippingMarkRequest.ShippingMarkId)
                .Where(x => x.ProductId == request.PrintShippingMarkRequest.ProductId)
                .Where(x => x.Status.Equals(nameof(ShippingMarkStatus.New)))
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            ///!TODO: handle Received Mark Printing

            if (shippingMarkPrintings == null || !shippingMarkPrintings.Any())
            {
                return null;
            }

            Entities.ShippingMarkPrinting printItem = null;

            for (int i = 0; i < shippingMarkPrintings.Count; i++)
            {
                var itemPrint = shippingMarkPrintings[i];

                if (itemPrint.PrintCount != 0)
                {
                    continue;
                }

                itemPrint.PrintCount += 1;
                itemPrint.Status = nameof(ShippingMarkStatus.Shipping);
                itemPrint.PrintingBy = request.PrintShippingMarkRequest.PrintedBy;
                itemPrint.PrintingDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                printItem = itemPrint;
                break;
            }

            var result = _mapper.Map<ShippingMarkPrintingModel>(printItem);
            
            if (result != null)
            {
                var shippingRequest = _mapper.Map<ShippingRequestModel>((await _context.ShippingMarkShippings
                    .Include(x => x.ShippingRequest)
                    .ThenInclude(x => x.ShippingRequestDetails)
                    .FirstOrDefaultAsync(x => x.ShippingMarkId == result.ShippingMarkId)).ShippingRequest);

                result.PrintInfo = new PrintInfomation
                {
                    PurchaseOrder = shippingRequest.ShippingRequestDetails.FirstOrDefault().PurchaseOrder,
                    ShippingRequest = shippingRequest,
                    TotalPackages = await _context.ShippingMarkPrintings.CountAsync(x => x.ProductId == result.ProductId && x.ShippingMarkId == result.ShippingMarkId),
                    Weight = 0,
                    WorkOrder = _mapper.Map<WorkOrderModel>((await _context.WorkOrderDetails
                        .Include(x => x.WorkOrder)
                        .FirstOrDefaultAsync(x => x.ProductId == result.ProductId)).WorkOrder)
                };
            }

            return result;
        }
    }
}

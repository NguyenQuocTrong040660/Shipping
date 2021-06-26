using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class RePrintShippingMarkCommand : IRequest<ShippingMarkPrintingModel>
    {
        public RePrintShippingMarkRequest RePrintShippingMarkRequest { get; set; }
    }

    public class RePrintShippingMarkCommandHandler : IRequestHandler<RePrintShippingMarkCommand, ShippingMarkPrintingModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public RePrintShippingMarkCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShippingMarkPrintingModel> Handle(RePrintShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var shippingMarkPrinting = await _context.ShippingMarkPrintings
                .Include(x => x.Product)
                .Include(x=> x.ShippingMark)
                .FirstOrDefaultAsync(x => x.Id == request.RePrintShippingMarkRequest.ShippingMarkPrintingId);

            if (shippingMarkPrinting == null)
            {
                return null;
            }

            shippingMarkPrinting.PrintCount += 1;
            shippingMarkPrinting.RePrintingBy = request.RePrintShippingMarkRequest.RePrintedBy;
            shippingMarkPrinting.RePrintingDate = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync() > 0 
                ? _mapper.Map<ShippingMarkPrintingModel>(shippingMarkPrinting) 
                : null;

            if (result != null)
            {
                var shippingRequest = _mapper.Map<ShippingRequestModel>((await _context.ShippingMarkShippings
                    .Include(x => x.ShippingRequest)
                    .ThenInclude(x => x.ShippingPlans)
                    .FirstOrDefaultAsync(x => x.ShippingMarkId == result.ShippingMarkId)).ShippingRequest);

                result.PrintInfo = new PrintInfomation
                {
                    PurchaseOrder = shippingRequest.ShippingPlans
                                            .FirstOrDefault(x => x.ProductId == result.ProductId).PurchaseOrder,
                    ShippingRequest = shippingRequest,
                    TotalPackages = await _context.ShippingMarkPrintings.CountAsync(x => x.ProductId == result.ProductId && x.ShippingMarkId == result.ShippingMarkId),
                    Weight = 0,
                    WorkOrder = _mapper.Map<WorkOrderModel>((await _context.WorkOrders.FirstOrDefaultAsync(x => x.ProductId == result.ProductId)))
                };
            }

            return result;
        }
    }
}


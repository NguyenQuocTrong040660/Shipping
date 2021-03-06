using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ShippingApp.Application.ReceivedMark.Queries;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ShippingMark.Queries
{
    public class GenerateShippingMarkShippingByShippingRequestQuery : IRequest<List<ShippingMarkShippingModel>>
    {
        public ShippingRequestModel ShippingRequest { get; set; }
    }

    public class GenerateShippingMarkShippingByShippingRequestQueryHandler : IRequestHandler<GenerateShippingMarkShippingByShippingRequestQuery, List<ShippingMarkShippingModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GenerateShippingMarkShippingByShippingRequestQueryHandler(IShippingAppDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<ShippingMarkShippingModel>> Handle(GenerateShippingMarkShippingByShippingRequestQuery request, 
            CancellationToken cancellationToken)
        {
            var shippingMarkShippings = new List<ShippingMarkShippingModel>();

            var shippingPlans = _mapper.Map<List<ShippingPlanModel>>(await _context.ShippingPlans
                     .AsNoTracking()
                     .Include(x => x.ShippingRequest)
                     .Include(x => x.Product)
                     .Where(x => x.ShippingRequestId.HasValue)
                     .Where(x => x.ShippingRequestId == request.ShippingRequest.Id)
                     .ToListAsync(cancellationToken));

            var shippingPlansGroupByProduct = shippingPlans
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    x.FirstOrDefault().Product,
                    TotalQuantity = x.Sum(x => x.Quantity),
                    x.FirstOrDefault().ShippingRequest,
                    x.FirstOrDefault().ShippingRequestId
                });

            foreach (var item in shippingPlansGroupByProduct)
            {
                item.Product.ReceivedMarkPrintings = await GetReceivedMarkPrintingsStorage(item.ProductId, cancellationToken);

                shippingMarkShippings.Add(new ShippingMarkShippingModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.TotalQuantity,
                    Product = item.Product,
                    ShippingRequest = item.ShippingRequest,
                    ShippingRequestId = item.ShippingRequestId.Value,
                    ShippingMarkId = 0
                });
            }

            return shippingMarkShippings;
        }

        private async Task<List<ReceivedMarkPrintingModel>> GetReceivedMarkPrintingsStorage(int productId, CancellationToken cancellationToken)
        {
            var receivedMarkPrintings = _mapper.Map<List<ReceivedMarkPrintingModel>>(await _context.ReceivedMarkPrintings
                        .AsNoTracking()
                        .Where(x => x.ProductId == productId)
                        .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.Storage)))
                        .OrderBy(x => x.Id)
                        .ToListAsync(cancellationToken));

            foreach (var receivedMarkPrinting in receivedMarkPrintings)
            {
                receivedMarkPrinting.WorkOrder = _mapper.Map<WorkOrderModel>(await _context.WorkOrders.FindAsync(receivedMarkPrinting.WorkOrderId));
            };

            return receivedMarkPrintings;
        }
    }
}

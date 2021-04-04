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

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GenerateReceivedMarkMovementsByMovementRequestsQuery : IRequest<List<ReceivedMarkMovementModel>>
    {
        public List<MovementRequestModel> MovementRequests { get; set; }
    }

    public class GenerateReceivedMarkMovementsByMovementRequestsQueryHandler : IRequestHandler<GenerateReceivedMarkMovementsByMovementRequestsQuery, List<ReceivedMarkMovementModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GenerateReceivedMarkMovementsByMovementRequestsQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ReceivedMarkMovementModel>> Handle(GenerateReceivedMarkMovementsByMovementRequestsQuery request, CancellationToken cancellationToken)
        {
            var receivedMarkMovements = new List<ReceivedMarkMovementModel>();

            //foreach (var item in request.MovementRequests)
            //{
            //    var movementRequestDetails = _mapper.Map<List<MovementRequestDetailModel>>(await _context.MovementRequestDetails
            //        .AsNoTracking()
            //        .Include(x => x.MovementRequest)
            //        .Include(x => x.Product)
            //        .Where(x => x.MovementRequestId == item.Id)
            //        .ToListAsync());

            //    var movementRequestDetailsGroupByProduct = movementRequestDetails
            //        .GroupBy(x => x.ProductId)
            //        .Select(x => new
            //        {
            //            ProductId = x.Key,
            //            x.FirstOrDefault().Product,
            //            TotalQuantity = x.Sum(x => x.Quantity),
            //            x.FirstOrDefault().MovementRequest,
            //            x.FirstOrDefault().MovementRequestId
            //        });

            //    receivedMarkMovements.AddRange(movementRequestDetailsGroupByProduct.Select(x => new ReceivedMarkMovementModel
            //    {
            //        ProductId = x.ProductId,
            //        Quantity = x.TotalQuantity,
            //        Product = x.Product,
            //        MovementRequest = x.MovementRequest,
            //        MovementRequestId = x.MovementRequestId,
            //        ReceivedMarkId = 0,
            //        WorkOrderId = string.Empty
            //    }).ToList());
            //}

            foreach (var item in request.MovementRequests)
            {
                var movementRequestDetails = await _context.MovementRequestDetails
                    .AsNoTracking()
                    .Include(x => x.Product)
                    .Include(x => x.WorkOrder)
                    .Include(x => x.MovementRequest)
                    .Where(x => x.MovementRequestId == item.Id)
                    .ToListAsync();

                receivedMarkMovements.AddRange(movementRequestDetails.Select(x => new ReceivedMarkMovementModel
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Product = _mapper.Map<ProductModel>(x.Product),
                    MovementRequest = _mapper.Map<MovementRequestModel>(x.MovementRequest),
                    MovementRequestId = x.MovementRequestId,
                    ReceivedMarkId = 0,
                    WorkOrderMomentRequest = $"{string.Concat(x.MovementRequest.Prefix, x.MovementRequest.Id ,"-", x.WorkOrder.RefId)}"
                }).ToList());
            }

            return receivedMarkMovements;
        }
    }
}

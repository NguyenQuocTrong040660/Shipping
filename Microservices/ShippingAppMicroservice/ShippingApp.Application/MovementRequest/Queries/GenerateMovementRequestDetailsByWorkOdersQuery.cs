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

namespace ShippingApp.Application.MovementRequest.Queries
{
    public class GenerateMovementRequestDetailsByWorkOdersQuery : IRequest<List<MovementRequestDetailModel>>
    {
        public List<WorkOrderModel> WorkOrderModels { get; set; }
    }
    public class GenerateMovementRequestDetailsByWorkOdersQueryHandler : IRequestHandler<GenerateMovementRequestDetailsByWorkOdersQuery, List<MovementRequestDetailModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GenerateMovementRequestDetailsByWorkOdersQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<MovementRequestDetailModel>> Handle(GenerateMovementRequestDetailsByWorkOdersQuery request, CancellationToken cancellationToken)
        {
            var movementRequestDetails = new List<MovementRequestDetailModel>();

            foreach (var item in request.WorkOrderModels)
            {
                var workOrderDetails = _mapper.Map<List<WorkOrderDetailModel>>(await _context.WorkOrderDetails
                    .AsNoTracking()
                    .Include(x => x.Product)
                    .Include(x => x.WorkOrder)
                    .ThenInclude(x => x.MovementRequestDetails)
                    .Where(x => x.WorkOrderId == item.Id)
                    .ToListAsync(cancellationToken));

                foreach (var workOrderDetail in workOrderDetails)
                {
                    var workOrder = workOrderDetail.WorkOrder;
                    workOrder.ReceviedMarkQuantity = await PopulateReceviedMarkQuantityAsync(workOrder.Id, cancellationToken);

                    movementRequestDetails.Add(new MovementRequestDetailModel
                    {
                        ProductId = workOrderDetail.ProductId,
                        Quantity = 0,
                        WorkOrderId = workOrderDetail.WorkOrderId,
                        WorkOrder = workOrder,
                        MovementRequestId = 0,
                        Product = workOrderDetail.Product,
                    });
                }
            }

            return movementRequestDetails;
        }

        private async Task<int> PopulateReceviedMarkQuantityAsync(int wokrOrderId, CancellationToken cancellationToken)
        {
            var receivedMarkMovements = await _context.ReceivedMarkMovements
                .Where(x => x.WorkOrderId == wokrOrderId)
                .ToListAsync(cancellationToken);

            return receivedMarkMovements.Sum(x => x.Quantity);
        }
    }
}

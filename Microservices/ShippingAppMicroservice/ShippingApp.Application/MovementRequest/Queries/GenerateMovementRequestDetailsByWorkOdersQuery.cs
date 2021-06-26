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
                var workOrder = _mapper.Map<WorkOrderModel>(await _context.WorkOrders
                    .AsNoTracking()
                    .Include(x => x.Product)
                    .ThenInclude(x => x.MovementRequestDetails)
                    .FirstOrDefaultAsync(x => x.Id == item.Id, cancellationToken));

                workOrder.ReceviedMarkQuantity = await PopulateReceviedMarkQuantityAsync(workOrder.Id, cancellationToken);

                movementRequestDetails.Add(new MovementRequestDetailModel
                {
                    ProductId = workOrder.ProductId,
                    Quantity = 0,
                    WorkOrderId = workOrder.Id,
                    WorkOrder = workOrder,
                    MovementRequestId = 0,
                    Product = workOrder.Product,
                });
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

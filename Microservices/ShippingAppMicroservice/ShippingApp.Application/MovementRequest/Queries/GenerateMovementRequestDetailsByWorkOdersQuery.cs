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
                    .Include(x => x.WorkOrder)
                    .Include(x => x.Product)
                    .Where(x => x.WorkOrderId == item.Id)
                    .ToListAsync());

                movementRequestDetails.AddRange(workOrderDetails.Select(x => new MovementRequestDetailModel
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    WorkOrderId = x.WorkOrderId,
                    WorkOrder = x.WorkOrder,
                    MovementRequestId = 0,
                    Product = x.Product,
                }).ToList());
            }

            return movementRequestDetails;
        }
    }
}

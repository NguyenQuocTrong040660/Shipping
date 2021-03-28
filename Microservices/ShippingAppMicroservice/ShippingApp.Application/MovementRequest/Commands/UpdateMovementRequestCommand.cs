using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShippingApp.Domain.Enumerations;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.MovementRequest.Commands
{
    public class UpdateMovementRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public MovementRequestModel MovementRequest { get; set; }
    }

    public class UpdateMovementRequestCommandHandler : IRequestHandler<UpdateMovementRequestCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public UpdateMovementRequestCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateMovementRequestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateMovementRequestAsync(request.MovementRequest);

            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            var movementRequest = await _context.MovementRequests
                .Include(x => x.MovementRequestDetails)
                .Where(x => x.Id == request.MovementRequest.Id)
                .FirstOrDefaultAsync();

            foreach (var item in movementRequest.MovementRequestDetails)
            {
                var movementRequestDetail = request.MovementRequest.MovementRequestDetails
                    .FirstOrDefault(i => i.ProductId == item.ProductId && i.WorkOrderId == item.WorkOrderId);

                if (movementRequestDetail == null)
                {
                    _context.MovementRequestDetails.Remove(item);
                }
                else
                {
                    item.Quantity = movementRequestDetail.Quantity;
                }
            }

            movementRequest.Notes = request.MovementRequest.Notes;

            await _context.SaveChangesAsync();
            await UpdateWorkOrderAsync(request.MovementRequest);

            return Result.Success();
        }

        private async Task UpdateWorkOrderAsync(MovementRequestModel entity)
        {
            foreach (var item in entity.MovementRequestDetails)
            {
                var workOrder = await _context.WorkOrders
                    .Include(x => x.WorkOrderDetails)
                    .Include(x => x.MovementRequestDetails)
                    .FirstOrDefaultAsync(i => i.Id == item.WorkOrderId);

                if (GetRemainQuantityWorkOrder(workOrder) == 0)
                {
                    workOrder.Status = nameof(WorkOrderStatus.Close);
                }
                else
                {
                    workOrder.Status = nameof(WorkOrderStatus.Start);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task<Result> ValidateMovementRequestAsync(MovementRequestModel model)
        {
            foreach (var item in model.MovementRequestDetails)
            {
                var workOrder = await _context.WorkOrders
                    .Include(x => x.WorkOrderDetails)
                    .Include(x => x.MovementRequestDetails)
                    .FirstOrDefaultAsync(i => i.Id == item.WorkOrderId);

                var remainQuantityOfWorkOrder = GetRemainQuantityWorkOrder(workOrder);

                if (item.Quantity > remainQuantityOfWorkOrder)
                {
                    return Result.Failure("Quantity can not be greater than Remain Quantity of Work Order");
                }
            }

            return Result.Success();
        }

        private int GetRemainQuantityWorkOrder(Entities.WorkOrder workOrder)
        {
            if (workOrder.WorkOrderDetails == null || workOrder.MovementRequestDetails == null || !workOrder.MovementRequestDetails.Any())
            {
                return workOrder.WorkOrderDetails.First().Quantity;
            }

            return workOrder.WorkOrderDetails.First().Quantity - workOrder.MovementRequestDetails.Sum(x => x.Quantity);
        }
    }
}

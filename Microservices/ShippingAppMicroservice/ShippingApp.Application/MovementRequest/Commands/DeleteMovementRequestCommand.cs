using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ShippingApp.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.MovementRequest.Commands
{
    public class DeleteMovementRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteMovementRequestCommandHandler : IRequestHandler<DeleteMovementRequestCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public DeleteMovementRequestCommandHandler(IShippingAppRepository<Entities.MovementRequest> shippingAppRepository,
           IShippingAppDbContext context)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(DeleteMovementRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await _shippingAppRepository.DeleteAsync(request.Id);

            if (result.Succeeded)
            {
                await UpdateWorkOrderAsync();
            }

            return result;
        }

        private async Task UpdateWorkOrderAsync()
        {
            var workOrders = await _context.WorkOrders
                    .Include(x => x.WorkOrderDetails)
                    .Include(x => x.MovementRequestDetails)
                    .ToListAsync();

            foreach (var item in workOrders)
            {
                if (GetRemainQuantityWorkOrder(item) == 0)
                {
                    item.Status = nameof(WorkOrderStatus.Close);
                }
                else
                {
                    item.Status = nameof(WorkOrderStatus.Start);
                }
            }

            await _context.SaveChangesAsync();
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

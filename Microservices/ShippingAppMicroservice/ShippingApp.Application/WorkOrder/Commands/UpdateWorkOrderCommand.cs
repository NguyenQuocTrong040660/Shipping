using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.WorkOrder.Commands
{
    public class UpdateWorkOrderCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public WorkOrderModel WorkOrder { get; set; }
    }

    public class UpdateWorkOrderCommandHandler : IRequestHandler<UpdateWorkOrderCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public UpdateWorkOrderCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Where(x => x.Id == request.WorkOrder.Id)
                .FirstOrDefaultAsync();
            
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            workOrder.Notes = request.WorkOrder.Notes;
            workOrder.RefId = request.WorkOrder.RefId;
            workOrder.Quantity = request.WorkOrder.Quantity;
            workOrder.ProductId = request.WorkOrder.ProductId;

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}

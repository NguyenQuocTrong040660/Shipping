using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.WorkOrder.Commands;
using System;
using System.Linq;

namespace ShippingApp.Application.WorkOrder.Validations
{
    public class DeleteWorkOrderCommandValidator : AbstractValidator<DeleteWorkOrderCommand>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteWorkOrderCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(x => x.Id)
                .Must(NotExistInMovementRequestDetails)
                .WithMessage("Can't delete Work Order already linked with Movement Request");
        }
        
        private bool NotExistInMovementRequestDetails(int workOrderId)
        {
            return _context.MovementRequestDetails.AsNoTracking().Any(x => x.WorkOrderId == workOrderId) == false;
        }
    }
}
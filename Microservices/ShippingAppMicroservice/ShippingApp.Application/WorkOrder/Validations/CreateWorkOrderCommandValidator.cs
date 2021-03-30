using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.WorkOrder.Commands;
using System;
using System.Linq;

namespace ShippingApp.Application.WorkOrder.Validations
{
    public class CreateWorkOrderCommandValidator : AbstractValidator<CreateWorkOrderCommand>
    {
        private readonly IShippingAppDbContext _context;

        public CreateWorkOrderCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(x => x.WorkOrder.RefId)
                .NotEmpty().NotNull()
                .Must(NotBeExisted)
                .WithMessage("RefId could not be null");
        }

        private bool NotBeExisted(string refId)
        {
            if (string.IsNullOrWhiteSpace(refId))
                return false;

            return _context.WorkOrders.AsNoTracking().Any(x => x.RefId.ToUpper() == refId.ToUpper()) == false;
        }
    }
}
using FluentValidation;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.ShippingPlan.Commands;
using System;

namespace ShippingApp.Application.ShippingPlan.Validations
{
    public class DeleteShippingPlanCommandValidator : AbstractValidator<DeleteShippingPlanCommand>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteShippingPlanCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
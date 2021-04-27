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
        public CreateWorkOrderCommandValidator()
        {
            RuleFor(x => x.WorkOrder.RefId)
                .NotEmpty().NotNull()
                .WithMessage("RefId could not be null");
        }
    }
}
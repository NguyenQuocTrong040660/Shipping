using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.ShippingMark.Commands;
using System;
using System.Linq;

namespace ShippingApp.Application.ShippingMark.Validations
{
    public class DeleteShippingMarkCommandValidator : AbstractValidator<DeleteShippingMarkCommand>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteShippingMarkCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(x => x.Id)
                .Must(NotExistInReceivedMarkPrintings)
                .Must(NotHaveAnyMarkPrinted)
                .WithMessage("Failed to delete shipping mark");
        }
        
        private bool NotExistInReceivedMarkPrintings(int shippingMarkId)
        {
            return _context.ReceivedMarkPrintings.AsNoTracking().Any(x => x.ShippingMarkId == shippingMarkId) == false;
        }

        private bool NotHaveAnyMarkPrinted(int shippingMarkId)
        {
            return _context.ShippingMarkPrintings.AsNoTracking().Any(x => x.ShippingMarkId == shippingMarkId && x.PrintCount != 0) == false;
        }
    }
}
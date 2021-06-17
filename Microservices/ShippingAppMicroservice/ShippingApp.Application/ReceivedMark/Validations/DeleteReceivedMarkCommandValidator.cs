using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.ReceivedMark.Commands;
using System;
using System.Linq;

namespace ShippingApp.Application.ReceivedMark.Validations
{
    public class DeleteReceivedMarkCommandValidator : AbstractValidator<DeleteReceivedMarkCommand>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteReceivedMarkCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(x => x.Id)
                .Must(NotHaveAnyMarkPrinted)
                .WithMessage("Can't delete Received Mark already printed");
        }
        
        private bool NotHaveAnyMarkPrinted(int receivedMarkId)
        {
            return _context.ReceivedMarkPrintings.AsNoTracking().Any(x => x.ReceivedMarkId == receivedMarkId && x.PrintCount != 0) == false;
        }
    }
}
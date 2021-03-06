using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.MovementRequest.Commands;
using System;
using System.Linq;

namespace ShippingApp.Application.MovementRequest.Validations
{
    public class DeleteMovementRequestCommandValidator : AbstractValidator<DeleteMovementRequestCommand>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteMovementRequestCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(x => x.Id)
                .Must(NotExistInReceivedMarkMovements)
                .WithMessage("Failed to delete movement request");
        }

        private bool NotExistInReceivedMarkMovements(int movementRequestId)
        {
            return _context.ReceivedMarkMovements.AsNoTracking().Any(x => x.MovementRequestId == movementRequestId) == false;
        }
    }
}
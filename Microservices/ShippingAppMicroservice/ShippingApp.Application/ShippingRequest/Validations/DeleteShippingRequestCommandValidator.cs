using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.ShippingRequest.Commands;
using System;
using System.Linq;

namespace ShippingApp.Application.ShippingRequest.Validations
{
    public class DeleteShippingRequestCommandValidator : AbstractValidator<DeleteShippingRequestCommand>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteShippingRequestCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));


            RuleFor(x => x.Id)
                .Must(NotExistInShippingMarkShippings)
                .WithMessage("Can't delete Shipping Request linked with Shipping Mark");
        }

        private bool NotExistInShippingMarkShippings(int shippingRequestId)
        {
            return _context.ShippingMarkShippings.AsNoTracking().Any(x => x.ShippingRequestId == shippingRequestId) == false;
        }
    }
}
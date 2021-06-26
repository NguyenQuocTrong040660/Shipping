using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.Product.Commands;
using System;
using System.Linq;

namespace ShippingApp.Application.Product.Validations
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        private readonly IShippingAppDbContext _context;

        public DeleteProductCommandValidator(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(x => x.Id)
                .Must(NotExistInShippingPlans)
                 .WithMessage("Can't delete Product already linked with Shipping Plan")

                .Must(NotExistInWorkOrders)
                 .WithMessage("Can't delete Product already linked with Work Order")

                .Must(NotExistInMovementRequestDetails)
                 .WithMessage("Can't delete Product already linked with Movement Request")

                .Must(NotExistInReceivedMarkMovements)
                .Must(NotExistInReceivedMarkPrintings)
                 .WithMessage("Can't delete Product already linked with Received Mark")

                .Must(NotExistInShippingMarkPrintings)
                .Must(NotExistInShippingMarkShippings)
                 .WithMessage("Can't delete Product already linked with Shipping Mark")

                .Must(NotExistInShippingRequestLogistics)
                 .WithMessage("Can't delete Product already linked with Shipping Request");
        }

        private bool NotExistInShippingPlans(int productId)
        {
            return _context.ShippingPlans.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }
        
        private bool NotExistInWorkOrders(int productId)
        {
            return _context.WorkOrders.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInMovementRequestDetails(int productId)
        {
            return _context.MovementRequestDetails.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInReceivedMarkMovements(int productId)
        {
            return _context.ReceivedMarkMovements.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInReceivedMarkPrintings(int productId)
        {
            return _context.ReceivedMarkPrintings.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingMarkPrintings(int productId)
        {
            return _context.ShippingMarkPrintings.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingMarkShippings(int productId)
        {
            return _context.ShippingMarkShippings.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingRequestLogistics(int productId)
        {
            return _context.ShippingRequestLogistics.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }
    }
}
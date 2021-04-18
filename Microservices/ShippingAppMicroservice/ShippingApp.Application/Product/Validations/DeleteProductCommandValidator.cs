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
                .Must(NotExistInShippingPlanDetails)
                .Must(NotExistInShippingRequestDetails)
                .Must(NotExistInWorkOrderDetails)
                .Must(NotExistInMovementRequestDetails)
                .Must(NotExistInReceivedMarkMovements)
                .Must(NotExistInReceivedMarkPrintings)
                .Must(NotExistInShippingMarkPrintings)
                .Must(NotExistInShippingMarkShippings)
                .Must(NotExistInShippingRequestLogistics)
                .WithMessage("Failed to delete product");
        }

        private bool NotExistInShippingPlanDetails(int productId)
        {
            return _context.ShippingPlanDetails.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingRequestDetails(int productId)
        {
            return _context.ShippingRequestDetails.AsNoTracking().Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInWorkOrderDetails(int productId)
        {
            return _context.WorkOrderDetails.AsNoTracking().Any(x => x.ProductId == productId) == false;
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
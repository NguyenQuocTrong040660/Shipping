using FluentValidation;
using Microsoft.AspNetCore.Http;
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
                .Must(NotExistInReceivedMarkSummaries)
                .Must(NotExistInShippingMarkPrintings)
                .Must(NotExistInShippingMarkShippings)
                .Must(NotExistInShippingMarkSummaries)
                .WithMessage("Failed to delete product");
        }

        private bool NotExistInShippingPlanDetails(int productId)
        {
            return _context.ShippingPlanDetails.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingRequestDetails(int productId)
        {
            return _context.ShippingRequestDetails.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInWorkOrderDetails(int productId)
        {
            return _context.WorkOrderDetails.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInMovementRequestDetails(int productId)
        {
            return _context.MovementRequestDetails.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInReceivedMarkMovements(int productId)
        {
            return _context.ReceivedMarkMovements.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInReceivedMarkPrintings(int productId)
        {
            return _context.ReceivedMarkPrintings.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInReceivedMarkSummaries(int productId)
        {
            return _context.ReceivedMarkSummaries.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingMarkPrintings(int productId)
        {
            return _context.ShippingMarkPrintings.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingMarkShippings(int productId)
        {
            return _context.ShippingMarkShippings.Any(x => x.ProductId == productId) == false;
        }

        private bool NotExistInShippingMarkSummaries(int productId)
        {
            return _context.ShippingMarkSummaries.Any(x => x.ProductId == productId) == false;
        }
    }
}
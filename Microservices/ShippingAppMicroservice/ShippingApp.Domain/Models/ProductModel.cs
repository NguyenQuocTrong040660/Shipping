using AutoMapper;
using FluentValidation;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ProductModel : AuditableEntityModel
    {
        [IgnoreMap]
        public string Identifier
        {
            get
            {
                return string.Concat(Prefix, Id);
            }
        }

        public int Id { get; set; }
        public string Prefix
        {
            get
            {
                return PrefixTable.Product;
            }
        }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Notes { get; set; }
        public string QtyPerPackage { get; set; }

        public string PartRevisionRaw { get; set; }
        public string PartRevisionClean { get; set; }
        public string ProcessRevision { get; set; }

        public virtual ICollection<ShippingPlanModel> ShippingPlans { get; set; }
        public virtual ICollection<MovementRequestDetailModel> MovementRequestDetails { get; set; }
        public virtual ICollection<ShippingRequestLogisticModel> ShippingRequestLogistics { get; set; }


        public virtual ICollection<WorkOrderModel> WorkOrders { get; set; }

        public virtual ICollection<ReceivedMarkMovementModel> ReceivedMarkMovements { get; set; }
        public virtual ICollection<ReceivedMarkPrintingModel> ReceivedMarkPrintings { get; set; }

        public virtual ICollection<ShippingMarkPrintingModel> ShippingMarkPrintings { get; set; }
        public virtual ICollection<ShippingMarkShippingModel> ShippingMarkShippings { get; set; }
    }

    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductNumber)
                .NotEmpty().NotNull().WithMessage("Please specify a Product Number");

            RuleFor(x => x.ProductName)
                .NotEmpty().NotNull().WithMessage("Please specify a Product Name");

            RuleFor(x => x.QtyPerPackage)
               .NotNull().WithMessage("Please specify a QtyPerPackage")
               .Must(BeNumber).WithMessage("QtyPerPackage must be a number");
        }

        private bool BeNumber(string arg)
        {
            return int.TryParse(arg, out _);
        }
    }
}

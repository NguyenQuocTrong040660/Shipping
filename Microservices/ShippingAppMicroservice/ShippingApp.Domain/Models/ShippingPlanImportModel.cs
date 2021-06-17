using FluentValidation;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ShippingPlanImportModel
    {
        public string ShippingPlanId
        {
            get
            {
                return string.Join("-", SalesOrder, SalelineNumber, ProductNumber);
            }
        }

        public string CustomerName { get; set; }
        public string PurchaseOrder { get; set; }
        public string SalesOrder { get; set; }
        public string SalelineNumber { get; set; }
        public string ProductNumber { get; set; }
        public string QuantityOrder { get; set; }
        public string SalesPrice { get; set; }
        public string ShippingMode { get; set; }
        public string ShippingDate { get; set; }
        public string Notes { get; set; }

        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }
        public string AccountNumber { get; set; }
        public int ProductLine { get; set; }
    }

    public class ShippingPlanImportModelValidator : AbstractValidator<ShippingPlanImportModel>
    {
        public ShippingPlanImportModelValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().NotNull().WithMessage("Please specify a CustomerName");

            RuleFor(x => x.ProductNumber)
                .NotEmpty().NotNull().WithMessage("Please specify a Product Number");

            RuleFor(x => x.PurchaseOrder)
                .NotEmpty().NotNull().WithMessage("Please specify a Purchase Order");

            RuleFor(x => x.SalesOrder)
               .NotEmpty().NotNull().WithMessage("Please specify a Sales Order");

            RuleFor(x => x.SalelineNumber)
              .NotEmpty().NotNull().WithMessage("Please specify a Saleline Number");

            RuleFor(x => x.QuantityOrder)
              .NotEmpty().NotNull().WithMessage("Please specify a Quantity Order")
              .Must(BeNumber).WithMessage("Quantity Order must be a number");

            RuleFor(x => x.SalesPrice)
              .NotEmpty().NotNull().WithMessage("Please specify a Sales Price")
              .Must(BeDecimal).WithMessage("SalesPrice must be a number");

            RuleFor(x => x.ShippingDate)
              .NotEmpty().NotNull().WithMessage("Please specify a Shipping Date")
              .Must(BeDate).WithMessage("ShippingDate must be a date");

            RuleFor(x => x.BillTo)
             .NotEmpty().NotNull().WithMessage("Please specify a BillTo");

            RuleFor(x => x.BillToAddress)
             .NotEmpty().NotNull().WithMessage("Please specify a BillToAddress");

            RuleFor(x => x.ShipTo)
             .NotEmpty().NotNull().WithMessage("Please specify a ShipTo");

            RuleFor(x => x.ShipToAddress)
             .NotEmpty().NotNull().WithMessage("Please specify a ShipToAddress");
        }

        private bool BeDate(string arg)
        {
            return DateTime.TryParse(arg, out _);
        }

        private bool BeDecimal(string arg)
        {
            return float.TryParse(arg, out _);
        }

        private bool BeNumber(string arg)
        {
            return int.TryParse(arg, out _);
        }
    }
}

using FluentValidation;

namespace ShippingApp.Domain.Models
{
    public class WorkOrderImportModel
    {
        public string WorkOrderId { get; set; }
        public string ProductNumber { get; set; }
        public string Quantity { get; set; }
        public string Notes { get; set; }
        public string PartRevision { get; set; }
        public string ProcessRevision { get; set; }
        public string CustomerName { get; set; }
    }

    public class WorkOrderImportModelValidator : AbstractValidator<WorkOrderImportModel>
    {
        public WorkOrderImportModelValidator()
        {
            RuleFor(x => x.WorkOrderId)
                .NotEmpty().NotNull().WithMessage("Please specify a Work Order Reference Id");

            RuleFor(x => x.ProductNumber)
                .NotEmpty().NotNull().WithMessage("Please specify a Product Number");

            RuleFor(x => x.Quantity)
               .NotNull().WithMessage("Please specify a Quantity")
               .Must(BeNumber).WithMessage("Quantity must be a number");
        }

        private bool BeNumber(string arg)
        {
            return int.TryParse(arg, out _);
        }
    }
}

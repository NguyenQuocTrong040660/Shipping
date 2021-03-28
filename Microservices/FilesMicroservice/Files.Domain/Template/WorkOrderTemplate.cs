using Files.Domain.Attributes;

namespace Files.Domain.Template
{
    public class WorkOrderTemplate
    {
        [ValidateDataType(IsRequired = true)]
        public string WorkOrderId { get; set; }
        [ValidateDataType(IsRequired = true)]
        public string ProductNumber { get; set; }
        [ValidateDataType(IsRequired = true, IsNumber = true)]
        public string Quantity { get; set; }

        public string PartRevision { get; set; }
        public string ProcessRevision { get; set; }
        public string CustomerName { get; set; }

        public string Notes { get; set; }
    }
}

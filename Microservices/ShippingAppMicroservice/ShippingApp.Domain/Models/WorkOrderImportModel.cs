namespace ShippingApp.Domain.Models
{
    public class WorkOrderImportModel
    {
        public string WorkOrderId { get; set; }
        public string ProductNumber { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public string PartRevision { get; set; }
        public string ProcessRevision { get; set; }
        public string CustomerName { get; set; }
    }
}

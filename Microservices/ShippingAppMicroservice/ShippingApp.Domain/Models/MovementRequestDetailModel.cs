namespace ShippingApp.Domain.Models
{
    public class MovementRequestDetailModel : AuditableEntityModel
    {
        public int WorkOrderId { get; set; }
        public virtual WorkOrderModel WorkOrder { get; set; }
        public int MovementRequestId { get; set; }
        public virtual MovementRequestModel MovementRequest { get; set; }
        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }
        public int Quantity { get; set; }
    }
}

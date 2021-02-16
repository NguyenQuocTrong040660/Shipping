namespace ShippingApp.Domain.Models
{
    public class MovementRequestDetailModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public virtual WorkOrderModel WorkOrder { get; set; }
        public int MovementRequestId { get; set; }
        public virtual MovementRequestModel MovementRequest { get; set; }
    }
}

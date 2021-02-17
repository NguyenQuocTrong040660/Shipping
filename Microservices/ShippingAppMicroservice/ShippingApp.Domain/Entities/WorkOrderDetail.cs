using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class WorkOrderDetail : AuditableEntity
    {
        public int Quantity { get; set; }
        public int WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}

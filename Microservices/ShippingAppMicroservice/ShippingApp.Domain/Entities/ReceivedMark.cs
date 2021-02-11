using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class ReceivedMark : AuditableEntity
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public int WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
    }
}

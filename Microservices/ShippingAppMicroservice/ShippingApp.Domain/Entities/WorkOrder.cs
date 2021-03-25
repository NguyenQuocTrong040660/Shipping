using ShippingApp.Domain.CommonEntities;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class WorkOrder : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string RefId { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        public virtual ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }
        public virtual ICollection<MovementRequestDetail> MovementRequestDetails { get; set; }
    }
}

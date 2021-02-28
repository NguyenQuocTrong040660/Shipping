using ShippingApp.Domain.CommonEntities;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class Product: AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string Notes { get; set; }
        public int QtyPerPackage { get; set; }

        public virtual ICollection<ShippingPlanDetail> ShippingPlanDetails { get; set; }
        public virtual ICollection<MovementRequestDetail> MovementRequestDetails { get; set; }
        public virtual ICollection<ShippingRequestDetail> ShippingRequestDetails { get; set; }
        public virtual ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }
        public virtual ICollection<ShippingMark> ShippingMarks { get; set; }
        public virtual ICollection<ReceivedMarkMovement> ReceivedMarkMovements { get; set; }
        public virtual ICollection<ReceivedMarkPrinting> ReceivedMarkPrintings { get; set; }
        public virtual ICollection<ReceivedMarkSummary> ReceivedMarkSummaries { get; set; }
    }
}

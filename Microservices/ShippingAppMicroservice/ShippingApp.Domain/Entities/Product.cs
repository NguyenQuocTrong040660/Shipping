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

        public string PartRevisionRaw { get; set; }
        public string PartRevisionClean { get; set; }
        public string ProcessRevision { get; set; }

        public virtual ICollection<ShippingPlan> ShippingPlans { get; set; }
        public virtual ICollection<MovementRequestDetail> MovementRequestDetails { get; set; }
        public virtual ICollection<ShippingRequestLogistic> ShippingRequestLogistics { get; set; }
        public virtual ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }
        public virtual ICollection<ReceivedMarkMovement> ReceivedMarkMovements { get; set; }
        public virtual ICollection<ReceivedMarkPrinting> ReceivedMarkPrintings { get; set; }
        public virtual ICollection<ShippingMarkPrinting> ShippingMarkPrintings { get; set; }
        public virtual ICollection<ShippingMarkShipping> ShippingMarkShippings { get; set; }
    }
}

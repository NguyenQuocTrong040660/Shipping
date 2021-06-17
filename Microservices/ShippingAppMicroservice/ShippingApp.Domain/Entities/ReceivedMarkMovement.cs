using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ReceivedMarkMovement : AuditableEntity
    {
        public int ReceivedMarkId { get; set; }
        public int ProductId { get; set; }
        public int MovementRequestId { get; set; }
        public int Quantity { get; set; }
        public int WorkOrderId { get; set; }
        public virtual ReceivedMark ReceivedMark { get; set; }
        public virtual MovementRequest MovementRequest { get; set; }
        public virtual Product Product { get; set; }
    }
}

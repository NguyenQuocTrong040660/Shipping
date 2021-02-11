using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class WorkOrder : AuditableEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int MovingQuantity { get; set; }
        public int RemainQuantity { get; set; }
        public string Notes { get; set; }
        public int MovementRequestId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual MovementRequest MovementRequest { get; set; }
        public virtual ICollection<ReceivedMark> ReceiveMarks { get; set; }
    }
}

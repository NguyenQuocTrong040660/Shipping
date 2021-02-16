﻿using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class MovementRequestDetail : AuditableEntity
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public int MovementRequestId { get; set; }
        public virtual MovementRequest MovementRequest { get; set; }
    }
}
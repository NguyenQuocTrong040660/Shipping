using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ShippingPlanDetail : AuditableEntity
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string Quantity { get; set; }
        public float Price { get; set; }
        public string ShippingMode { get; set; }
        public float Amount { get; set; }
        public int ShippingPlanId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ShippingPlan ShippingPlan { get; set; }
    }
}

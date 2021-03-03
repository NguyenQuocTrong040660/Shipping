using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class ShippingPlan: AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string PurchaseOrder { get; set; }
        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string SalesID { get; set; }
        public int SemlineNumber { get; set; }
        public string RefId { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<ShippingPlanDetail> ShippingPlanDetails { get; set; }
    }
}

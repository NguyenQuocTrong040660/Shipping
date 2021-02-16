using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ShippingPlanModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string SalesID { get; set; }
        public int SemlineNumber { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<ShippingPlanDetailModel> ShippingPlanDetails { get; set; }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ProductModel : AuditableEntityModel
    {
        [IgnoreMap]
        public string Identifier
        {
            get
            {
                return string.Concat(Prefix, Id);
            }
        }

        public int Id { get; set; }
        public string Prefix { get; set; }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Notes { get; set; }
        public string QtyPerPackage { get; set; }

        public virtual ICollection<ShippingPlanDetailModel> ShippingPlanDetails { get; set; }
        public virtual ICollection<ShippingRequestDetailModel> ShippingRequestDetails { get; set; }
        public virtual ICollection<WorkOrderDetailModel> WorkOrderDetails { get; set; }
        public virtual ICollection<ShippingMarkModel> ShippingMarks { get; set; }
        public virtual ICollection<ReceivedMarkModel> ReceivedMarks { get; set; }
    }
}

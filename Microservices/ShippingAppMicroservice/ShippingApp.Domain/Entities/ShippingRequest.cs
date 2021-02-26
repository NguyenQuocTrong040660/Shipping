using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class ShippingRequest : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string SalesID { get; set; }
        public int SemlineNumber { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<ShippingRequestLogistic> ShippingRequestLogistics { get; set; }
        public virtual ICollection<ShippingRequestDetail> ShippingRequestDetails { get; set; }
        public virtual ICollection<ShippingMark> ShippingMarks { get; set; }
    }
}

using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class ShippingRequest : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        public int AccountNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public string CustomerName { get; set; }
        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }

        public virtual ICollection<ShippingRequestLogistic> ShippingRequestLogistics { get; set; }
        public virtual ICollection<ShippingRequestDetail> ShippingRequestDetails { get; set; }
        public virtual ICollection<ShippingMarkShipping> ShippingMarkShippings { get; set; }
    }
}

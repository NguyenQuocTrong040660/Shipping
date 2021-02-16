using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ShippingMark : AuditableEntity
    {
        public int Id { get; set; }
        public string Revision { get; set; }
        public int Quantity { get; set; }
        public int Sequence { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int ProductId { get; set; }
        public int PrintCount { get; set; }
        public int ShippingRequestId { get; set; }
        public virtual ShippingRequest ShippingRequest { get; set; }
        public virtual Product Product { get; set; }
    }
}

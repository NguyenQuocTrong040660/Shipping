using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ShippingMark : AuditableEntity
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string ProductNumber { get; set; }
        public string Revision { get; set; }
        public string CartonNumber { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}

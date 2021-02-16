using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ReceivedMark : AuditableEntity
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintBy { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}

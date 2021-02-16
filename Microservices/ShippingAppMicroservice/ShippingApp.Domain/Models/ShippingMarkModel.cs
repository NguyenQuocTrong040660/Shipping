using System;

namespace ShippingApp.Domain.Models
{
    public class ShippingMarkModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Revision { get; set; }
        public string CartonNumber { get; set; }
        public int Quantity { get; set; }
        public int Sequence { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintBy { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}

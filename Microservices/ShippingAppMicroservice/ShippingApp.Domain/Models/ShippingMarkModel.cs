using System;

namespace ShippingApp.Domain.Models
{
    public class ShippingMarkModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string ProductNumber { get; set; }
        public string Revision { get; set; }
        public string CartonNumber { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }

        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}

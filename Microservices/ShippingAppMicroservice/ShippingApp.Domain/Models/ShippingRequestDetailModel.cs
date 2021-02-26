using System;

namespace ShippingApp.Domain.Models
{
    public class ShippingRequestDetailModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ShippingMode { get; set; }
        public float Amount { get; set; }
        public int ShippingRequestId { get; set; }
        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual ShippingRequestModel ShippingRequest { get; set; }
    }
}

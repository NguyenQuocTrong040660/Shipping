using System;

namespace ShippingApp.Domain.Models
{
    public class ShippingMarkModel : AuditableEntityModel
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
        public virtual ShippingRequestModel ShippingRequest { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}

using AutoMapper;

namespace ShippingApp.Domain.Models
{
    public class ShippingMarkShippingModel : AuditableEntityModel
    {
        public int ShippingMarkId { get; set; }
        public int ProductId { get; set; }
        public int ShippingRequestId { get; set; }
        public int Quantity { get; set; }

        public virtual ShippingMarkModel ShippingMark { get; set; }
        public virtual ShippingRequestModel ShippingRequest { get; set; }
        public virtual ProductModel Product { get; set; }

        [IgnoreMap]
        public int TotalQuantity { get; set; }
        [IgnoreMap]
        public int TotalPackage { get; set; }
        [IgnoreMap]
        public int TotalQuantityPrinted { get; set; }
    }
}

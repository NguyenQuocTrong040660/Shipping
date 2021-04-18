using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ShippingRequestDetail : AuditableEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ShippingMode { get; set; }
        public float Amount { get; set; }

        public string PurchaseOrder { get; set; }
        public string SalesOrder { get; set; }
        public string SalelineNumber { get; set; }
        public int ProductLine { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int ShippingRequestId { get; set; }
        public virtual ShippingRequest ShippingRequest { get; set; }
    }
}

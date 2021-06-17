using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ShippingPlan : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string PurchaseOrder { get; set; }
        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string SalesOrder { get; set; }
        public string SalelineNumber { get; set; }
        public string Notes { get; set; }

        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }
        public string AccountNumber { get; set; }
        public int ProductLine { get; set; }

        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ShippingMode { get; set; }
        public float Amount { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int? ShippingRequestId { get; set; }
        public virtual ShippingRequest ShippingRequest { get; set; }
    }
}

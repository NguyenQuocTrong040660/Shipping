using System;
using System.Collections.Generic;

namespace Communication.Domain.Models
{
    public class ShippingRequestResponse
    {
        public string ShippingDeptEmail { get; set; }
        public string LogisticDeptEmail { get; set; }
        public ShippingRequestModel ShippingRequest { get; set; }
    }

    public class ShippingRequestModel
    {
        public string Identifier { get; set; }
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string SalesOrder { get; set; }
        public string SalelineNumber { get; set; }
        public string Notes { get; set; }
        public string PurchaseOrder { get; set; }
        public int ShippingRequestLogisticId { get; set; }
        public virtual ICollection<ShippingPlanModel> ShippingPlans { get; set; }
    }

    public class ShippingPlanModel
    {
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ShippingMode { get; set; }
        public float Amount { get; set; }
        public virtual ProductModel Product { get; set; }
    }

    public class ProductModel
    {
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Notes { get; set; }
        public string QtyPerPackage { get; set; }
    }
}

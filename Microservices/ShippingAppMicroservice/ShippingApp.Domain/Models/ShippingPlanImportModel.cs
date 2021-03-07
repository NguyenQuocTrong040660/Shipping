using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ShippingPlanImportModel
    {
        public string ShippingPlanId { get; set; }
        public string CustomerName { get; set; }
        public string PurchaseOrder { get; set; }
        public string SalesID { get; set; }
        public string SemlineNumber { get; set; }
        public string ProductNumber { get; set; }
        public int QuantityOrder { get; set; }
        public float SalesPrice { get; set; }
        public string ShippingMode { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Notes { get; set; }
    }
}

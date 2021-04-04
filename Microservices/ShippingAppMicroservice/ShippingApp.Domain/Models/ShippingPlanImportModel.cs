using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ShippingPlanImportModel
    {
        public string ShippingPlanId
        {
            get
            {
                return string.Join("-", SalesOrder, SalelineNumber, ProductNumber);
            }
        }

        public string CustomerName { get; set; }
        public string PurchaseOrder { get; set; }
        public string SalesOrder { get; set; }
        public string SalelineNumber { get; set; }
        public string ProductNumber { get; set; }
        public int QuantityOrder { get; set; }
        public float SalesPrice { get; set; }
        public string ShippingMode { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Notes { get; set; }

        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }
        public int AccountNumber { get; set; }
        public int ProductLine { get; set; }
    }
}

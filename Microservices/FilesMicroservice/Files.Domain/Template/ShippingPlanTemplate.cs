using Files.Domain.Attributes;
using System;

namespace Files.Domain.Template
{
    public class ShippingPlanTemplate
    {
        [ValidateDataType(IsRequired = true)]
        public string CustomerName { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string PurchaseOrder { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string SalesOrder { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string SalelineNumber { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string ProductNumber { get; set; }

        [ValidateDataType(IsNumber = true)]
        public string QuantityOrder { get; set; }

        [ValidateDataType(IsDecimal = true)]
        public string SalesPrice { get; set; }
        public string ShippingMode { get; set; }

        [ValidateDataType(IsDateTime = true)]
        public string ShippingDate { get; set; }   
        public string Notes { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string BillTo { get; set; }
        [ValidateDataType(IsRequired = true)]
        public string BillToAddress { get; set; }
        [ValidateDataType(IsRequired = true)]
        public string ShipTo { get; set; }
        [ValidateDataType(IsRequired = true)]
        public string ShipToAddress { get; set; }
        public int AccountNumber { get; set; }
        public int ProductLine { get; set; }
    }
}

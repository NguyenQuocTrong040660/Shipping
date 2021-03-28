using Files.Domain.Attributes;
using System;

namespace Files.Domain.Template
{
    public class ShippingPlanTemplate
    {
        [ValidateDataType(IsRequired = true)]

        public string ShippingPlanId
        {
            get
            {
                return string.Join("-", SalesID, SemlineNumber, ProductNumber);
            }
        }
        
        [ValidateDataType(IsRequired = true)]
        public string CustomerName { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string PurchaseOrder { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string SalesID { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string SemlineNumber { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string ProductNumber { get; set; }

        [ValidateDataType(IsNumber = true)]
        public int QuantityOrder { get; set; }

        [ValidateDataType(IsDecimal = true)]
        public float SalesPrice { get; set; }
        public string ShippingMode { get; set; }

        [ValidateDataType(IsDateTime = true)]
        public DateTime ShippingDate { get; set; }   
        public string Notes { get; set; }

        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }
        public string AccountNumber { get; set; }
        public string ProductLine { get; set; }
    }
}

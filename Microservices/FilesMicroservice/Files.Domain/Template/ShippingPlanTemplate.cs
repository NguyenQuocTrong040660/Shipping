﻿using Files.Domain.Attributes;
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
        public string SalesID { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string SemlineNumber { get; set; }

        [ValidateDataType(IsRequired = true)]
        public string ProductNumber { get; set; }

        [ValidateDataType(IsNumber = true)]
        public int QuantityOrder { get; set; }

        [ValidateDataType(IsNumber = true)]
        public double SalesPrice { get; set; }
        public string ShippingMode { get; set; }
        public DateTime ShippingDate { get; set; }   
        public string Notes { get; set; }
    }
}
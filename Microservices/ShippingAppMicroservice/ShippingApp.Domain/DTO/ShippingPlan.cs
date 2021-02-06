﻿using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.DTO
{
    public class ShippingPlan
    {
        public Guid Id { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public int SemlineNumber { get; set; }
        public string PurchaseOrder { get; set; }
        public int QuantityOrder { get; set; }
        public float SalesPrice { get; set; }
        public string CustomerName { get; set; }
        public string SalesID { get; set; }
        public string ShippingMode { get; set; }
        public string ShippingDate { get; set; }
        public string Notes { get; set; }
    }
}
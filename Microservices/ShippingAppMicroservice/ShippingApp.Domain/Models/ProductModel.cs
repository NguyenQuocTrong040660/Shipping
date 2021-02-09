using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Notes { get; set; }
        public string QtyPerPackage { get; set; }
    }
}

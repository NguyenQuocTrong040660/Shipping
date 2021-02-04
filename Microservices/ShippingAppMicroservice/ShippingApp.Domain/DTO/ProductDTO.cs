using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Note { get; set; }
        public string QtyPerPackage { get; set; }
    }
}

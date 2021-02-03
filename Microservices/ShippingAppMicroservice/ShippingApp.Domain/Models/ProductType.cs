using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Models
{
    public class ProductType
    {
        public ProductType Model;

        public Guid Id { get; set; }
        public string ProductTypeName { get; set; }
        public int ProductTotal { get; set; }
        public int CompanyIndex { get; set; }
        public string CompanyName { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}

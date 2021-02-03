using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Models
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public int CompanyIndex { get; set; }
        public string CompanyName { get; set; }

        public DateTime Created { get; set; }
         public DateTime? LastModified { get; set; }
        
    }
}


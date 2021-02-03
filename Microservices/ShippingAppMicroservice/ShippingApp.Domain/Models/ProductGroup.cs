using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ProductGroup
    {
        public List<Country> Countries { get; set; }
        public List<Brand> Brands { get; set; }
    }
}

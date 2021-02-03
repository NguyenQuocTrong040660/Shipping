using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class ProductGeneral : AuditableEntity
    {
        public string ID { get; set; }
        public string ProducerCompany { get; set; }
        public string Manufactory { get; set; }
        public string DistributorCompany { get; set; }
        public string Country { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public ProductOverview ProductOverview { get; set; }
    }
}

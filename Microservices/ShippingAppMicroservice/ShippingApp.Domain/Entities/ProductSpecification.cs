using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class ProductSpecification: AuditableEntity
    {
        public string ID { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public virtual ProductOverview ProductOverview { get; set; }
    }
}

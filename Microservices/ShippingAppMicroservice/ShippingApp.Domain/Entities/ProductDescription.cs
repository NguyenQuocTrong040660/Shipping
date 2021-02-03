using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class ProductDescription : AuditableEntity
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public virtual ProductOverview ProductOverview { get; set; }
    }
}

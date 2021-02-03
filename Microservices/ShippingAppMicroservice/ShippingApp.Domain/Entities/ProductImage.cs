using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class ProductImage : AuditableEntity
    {
        public string ID { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public string ImageUrl { get; set; }
        public string ImageBase64Encrypt { get; set; }
        public virtual ProductOverview ProductOverview { get; set; }
    }
}

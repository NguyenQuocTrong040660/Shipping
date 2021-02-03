using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class ProductType: AuditableEntity
    {
        public Guid Id { get; set; }
        public string ProductTypeName { get; set; }
        public int CompanyIndex { get; set; }

    }
}

using ShippingApp.Domain.CommonEntities;
using System;
using System.Diagnostics.Tracing;

namespace ShippingApp.Domain.Entities
{
    public class Brand : AuditableEntity
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public int CompanyIndex { get; set; }
    }
}

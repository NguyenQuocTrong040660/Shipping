using System;

namespace ShippingApp.Domain.CommonEntities
{
    public abstract class AuditableEntity
    {
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}

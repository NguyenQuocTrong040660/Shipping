using System;

namespace UserManagement.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    }
}

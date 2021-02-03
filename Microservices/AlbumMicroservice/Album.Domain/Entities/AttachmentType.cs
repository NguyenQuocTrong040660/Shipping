using Album.Domain.CommonEntities;
using System;
using System.Collections.Generic;

namespace Album.Domain.Entities
{
    public class AttachmentType : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}

using Album.Domain.CommonEntities;
using System;

namespace Album.Domain.Entities
{
    public class Attachment : AuditableEntity
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public long? FileSize { get; set; }
        public Guid AttachmentTypeId { get;set;}

        public virtual AttachmentType AttachmentType { get; set; }
    }
}

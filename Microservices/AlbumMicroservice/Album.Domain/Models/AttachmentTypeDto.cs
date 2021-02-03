using System;

namespace Album.Domain.Models
{
    public class AttachmentTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }
}

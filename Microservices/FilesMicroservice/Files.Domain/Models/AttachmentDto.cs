using AutoMapper;
using System;

namespace Files.Domain.Models
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public long? FileSize { get; set; }
        public DateTime Created { get; set; }
        public Guid AttachmentTypeId { get; set; }
        public AttachmentTypeDto AttachmentType { get; set; }

        [IgnoreMap]
        public string AttachmentTypeName
        {
            get
            {
                if (AttachmentType != null)
                {
                    return AttachmentType.Name;
                }

                return string.Empty;
            }
            set { }
        }
    }
}

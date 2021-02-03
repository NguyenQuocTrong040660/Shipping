﻿using Album.Domain.Enumerations;
using Microsoft.AspNetCore.Http;

namespace Album.Domain.Models
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
        public string AttachmentTypeId { get; set; }
    }
}

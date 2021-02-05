using Files.Domain.Enumerations;
using Microsoft.AspNetCore.Http;

namespace Files.Domain.Models
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
        public string AttachmentTypeId { get; set; }
    }
}

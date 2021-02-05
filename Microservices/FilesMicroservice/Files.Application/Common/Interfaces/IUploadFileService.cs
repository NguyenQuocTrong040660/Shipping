using Files.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Files.Application.Common.Interfaces
{
    public interface IUploadFileService
    {
        (Result, string, string) UploadFile(IFormFile file, string domain, string webRoot, string attachmentTypes);
        (Result, string, string) UploadFileWithAttachmentTypeId(IFormFile file, string domain, string webRoot, string attachmentTypeId);
        void DeleteFile(string webRoot, string attachmentTypeId, string fileName);
    }
}

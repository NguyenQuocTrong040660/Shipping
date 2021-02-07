using Files.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Files.Application.Common.Interfaces
{
    public interface IUploadFileService
    {
        (Result, string, string) UploadFile(IFormFile file, string domain, string attachmentTypes);
        (Result, string, string) UploadFileWithAttachmentTypeId(IFormFile file, string domain, string attachmentTypeId);
        void DeleteFile(string attachmentTypeId, string fileName);
    }
}

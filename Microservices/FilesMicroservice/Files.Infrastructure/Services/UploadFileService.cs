using Files.Application.Common.Interfaces;
using Files.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using System.Linq;
using Files.Domain.Enumerations;

namespace Files.Infrastructure.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IFilesDbContext _dbContext;
        private readonly ILogger<UploadFileService> _logger;

        private const string FolderUploadPhoto = "photos";
        private const string FolderUploadVideo = "videos";
        private const long MaximumFileSizePhoto = 2097152000;
        private const long MaximumFileSizeVideo = 20971520000;

        private const string ErrorMaxFileSize = "File size is too heavy. Please choice another file";
        private static readonly ReadOnlyCollection<string> AllowPhotoType = new ReadOnlyCollection<string>(
                                                                              new string[] { ".png", ".jpg", ".jpeg", });

        private static readonly ReadOnlyCollection<string> AllowVideoType = new ReadOnlyCollection<string>(
                                                                              new string[] {  ".mp4", ".avi" });
        public UploadFileService(IFilesDbContext dbContext,
             ILogger<UploadFileService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public (Result, string, string) UploadFile(IFormFile file, string domain, string webRoot, string attachmentType)
        {
            string fileUrl = string.Empty;
            string fileName = string.Empty;

            if (file == null || file.Length == 0)
            {
                return (Result.Failure("File can not be null"), fileUrl, fileName);
            }

            long fileSizeLimit = GetFileSizeLimitByType(attachmentType);

            if (file.Length > fileSizeLimit)
            {
                return (Result.Failure(ErrorMaxFileSize), fileUrl, fileName);
            }

            string extension = Path.GetExtension(file.FileName);

            if (attachmentType == AttachmentTypes.Photo && !AllowPhotoType.Contains(extension))
            {
                return (Result.Failure("File type not valid"), fileUrl, fileName);
            }

            if (attachmentType == AttachmentTypes.Video && !AllowVideoType.Contains(extension))
            {
                return (Result.Failure("File type not valid"), fileUrl, fileName);
            }

            try
            {
                string folderToUploads = GetFolderToUploadByType(attachmentType);
                string folderName = Path.Combine(webRoot, folderToUploads);
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                fileName = GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                                .FileName);

                string fullPath = Path.Combine(pathToSave, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                fileUrl = $"/{folderToUploads}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return (Result.Success(), fileUrl, fileName);
        }

        public void DeleteFile(string webRoot, string attachmentTypeId, string fileName)
        {
            var attachmentTypeEntity = _dbContext.AttachmentTypes.FirstOrDefault(x => Equals(x.Id, new Guid(attachmentTypeId)));

            if (attachmentTypeEntity == null)
            {
                throw new ArgumentNullException(nameof(attachmentTypeEntity));
            }

            string folderToUploads = GetFolderToUploadByType(attachmentTypeEntity.Name);
            string folderName = Path.Combine(webRoot, folderToUploads);
            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            string fullPath = Path.Combine(pathFile, fileName);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch (IOException e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }

        public (Result, string, string) UploadFileWithAttachmentTypeId(IFormFile file, string domain, string webRoot, string attachmentTypeId)
        {
            var attachmentTypeEntity = _dbContext.AttachmentTypes.FirstOrDefault(x => Equals(x.Id, new Guid(attachmentTypeId)));

            if (attachmentTypeEntity == null)
            {
                throw new ArgumentNullException(nameof(attachmentTypeEntity));
            }

            return UploadFile(file, domain , webRoot, attachmentTypeEntity.Name);
        }

        public static long GetFileSizeLimitByType(string attachmentTypes)
        {
            return attachmentTypes switch
            {
                AttachmentTypes.Photo => MaximumFileSizePhoto,
                AttachmentTypes.Video => MaximumFileSizeVideo,
                _ => throw new NotSupportedException()
            };
        }
        public static string GetFolderToUploadByType(string attachmentTypes)
        {
            return attachmentTypes switch
            {
                AttachmentTypes.Photo => FolderUploadPhoto,
                AttachmentTypes.Video => FolderUploadVideo,
                _ => throw new NotSupportedException()
            };
        }
        public static string GetRandomString()
        {
            Random random = new Random();

            return DateTime.Now.Day.ToString()
                + "_" + DateTime.Now.Month.ToString()
                + "_" + DateTime.Now.Year.ToString()
                + DateTime.Now.Second.ToString()
                + "_" + random.Next();
        }
        public static string GetFileName(string fileName) => GetRandomString() + fileName.Trim('"');
    }
}

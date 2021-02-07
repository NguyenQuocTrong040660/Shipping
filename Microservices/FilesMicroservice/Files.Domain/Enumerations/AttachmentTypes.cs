using System;
using System.Collections.ObjectModel;

namespace Files.Domain.Enumerations
{
    public struct AttachmentTypes
    {
        public const string Photo = nameof(Photo);
        public const string Video = nameof(Video);
        public const string Excel = nameof(Excel);

        public static ReadOnlyCollection<string> GetAllowAttachmentTypes(string attachmentType)
        {
            return attachmentType switch
            {
                Photo => new ReadOnlyCollection<string>(new string[] { ".png", ".jpg", ".jpeg" }),
                Video => new ReadOnlyCollection<string>(new string[] { ".mp4", ".avi" }),
                Excel => new ReadOnlyCollection<string>(new string[] { ".xls", ".xlsx" }),
                _ => throw new NotSupportedException()
            };
        }

        public static string GetFolderToUploadByType(string attachmentTypes)
        {
            return attachmentTypes switch
            {
                Photo => "photos",
                Video => "videos",
                Excel => "files",
                _ => throw new NotSupportedException()
            };
        }

        public static long GetFileSizeLimitByType(string attachmentTypes)
        {
            return attachmentTypes switch
            {
                Photo => 2097152000,
                Video => 20971520000,
                Excel => 2097152000,
                _ => throw new NotSupportedException()
            };
        }
    }
}

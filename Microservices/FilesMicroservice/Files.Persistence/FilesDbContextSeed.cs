using Files.Domain.Entities;
using Files.Domain.Enumerations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Files.Persistence.DBContext
{
    public static class FilesDbContextSeed
    {
        public static async Task SeedAttachmentTypesAsync(FilesDbContext context)
        {
            // Seed, if necessary
            if (!context.AttachmentTypes.Any())
            {
                var attachmentTypes = new List<AttachmentType>
                {
                    new AttachmentType
                    {
                        Name = AttachmentTypes.Photo,
                        
                    },
                    new AttachmentType
                    {
                       Name = AttachmentTypes.Video,
                    },
                };

                context.AttachmentTypes.AddRange(attachmentTypes);
                await context.SaveChangesAsync();
            }
        }
    }
}

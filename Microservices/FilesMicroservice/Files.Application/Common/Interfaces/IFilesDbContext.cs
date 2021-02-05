using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Files.Domain.Entities;

namespace Files.Application.Common.Interfaces
{
    public interface IFilesDbContext
    {
        DbSet<Attachment> Attachments { get; }
        DbSet<Files.Domain.Entities.AttachmentType> AttachmentTypes { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}

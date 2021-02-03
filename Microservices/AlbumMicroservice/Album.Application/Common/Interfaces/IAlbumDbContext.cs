using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Album.Domain.Entities;

namespace Album.Application.Common.Interfaces
{
    public interface IAlbumDbContext
    {
        DbSet<VideoHomePage> VideoHomePages { get; }
        DbSet<Attachment> Attachments { get; }
        DbSet<Album.Domain.Entities.AttachmentType> AttachmentTypes { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}

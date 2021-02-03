using Microsoft.EntityFrameworkCore;
using Album.Domain.CommonEntities;
using Album.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Album.Application.Common.Interfaces;

namespace Album.Persistence.DBContext
{
    public class AlbumDbContext : DbContext, IAlbumDbContext
    {
        public AlbumDbContext(DbContextOptions<AlbumDbContext> options) : base(options) 
        { 
        }

        public DbSet<VideoHomePage> VideoHomePages { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentType> AttachmentTypes { get; set; }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoHomePage>(entity => {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(256);
                entity.Property(e => e.Descriptions).HasMaxLength(256);
                entity.Property(e => e.YoutubeId).HasMaxLength(100);
                entity.Property(e => e.YoutubeImage).HasMaxLength(256);
                entity.Property(e => e.YoutubeLink).HasMaxLength(256);
            });

            modelBuilder.Entity<AttachmentType>(entity => {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Attachment>(entity => {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(newid())");
                entity.Property(e => e.FileName).HasMaxLength(256);
                entity.Property(e => e.FilePath).HasMaxLength(256);
                entity.Property(e => e.FileType).HasMaxLength(256);
                entity.Property(e => e.FileUrl).HasMaxLength(500);
            });
        }
    }
}

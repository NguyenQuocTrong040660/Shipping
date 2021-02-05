using Microsoft.EntityFrameworkCore;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.CommonEntities;
using System;
using System.Threading.Tasks;
using System.Threading;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.DBContext
{
    public class ShippingAppDbContext : DbContext, IShippingAppDbContext
    {
        public ShippingAppDbContext(DbContextOptions<ShippingAppDbContext> options): base(options)
        { 
        }

        public DbSet<Entities.ProductType> ProductType { get; set; }
        public DbSet<Entities.Country> Country { get; set; }
        public DbSet<Entities.ProductEntity> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.ProductType>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Entities.Country>()
                .HasKey(c => new { c.CountryCode });

            modelBuilder.Entity<Entities.ProductEntity>()
               .HasKey(c => new { c.Id });

            base.OnModelCreating(modelBuilder);
        }

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
    }
}

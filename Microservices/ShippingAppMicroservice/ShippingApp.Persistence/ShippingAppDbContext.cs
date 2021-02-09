using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
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

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShippingPlan> ShippingPlans { get; set; }

        public DbSet<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductType>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Country>()
                .HasKey(c => new { c.CountryCode });

            modelBuilder.Entity<Product>()
               .HasKey(c => new { c.Id });

            modelBuilder.Entity<ShippingPlan>()
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

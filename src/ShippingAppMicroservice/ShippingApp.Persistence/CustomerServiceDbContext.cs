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
        public DbSet<Entities.ProductDescription> ProductDescription { get; set; }
        public DbSet<Entities.ProductImage> ProductImage { get; set; }
        public DbSet<Entities.ProductGeneral> ProductGeneral { get; set; }
        public DbSet<Entities.ProductSpecification> ProductSpecification { get; set; }
        public DbSet<Entities.ProductOverview> ProductOverview { get; set; }
        public DbSet<Entities.Country> Country { get; set; }
        public DbSet<Entities.Brand> Brand { get; set; }

        public DbSet<Entities.Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.ProductDescription>()
                .HasKey(c => new { c.ID });

            modelBuilder.Entity<Entities.ProductSpecification>()
                .HasKey(c => new { c.ID});

            modelBuilder.Entity<Entities.ProductImage>()
                .HasKey(c => new { c.ID });

           modelBuilder.Entity<Entities.ProductGeneral>()
                .HasKey(c => new { c.ID });

            modelBuilder.Entity<Entities.ProductType>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Entities.ProductOverview>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Entities.Brand>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Entities.Country>()
                .HasKey(c => new { c.CountryCode });

            modelBuilder.Entity<Entities.Order>()
                .HasKey(c => new { c.Id });
            modelBuilder.Entity<Entities.Customer>()
                .HasKey(c => new { c.Id });
            modelBuilder.Entity<Entities.OrderDetail>()
                .HasKey(c => new { c.OrderId });
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

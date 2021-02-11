using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.CommonEntities;
using System;
using System.Threading.Tasks;
using System.Threading;
using ShippingApp.Domain.Entities;
using System.Reflection;

namespace ShippingApp.Persistence.DBContext
{
    public class ShippingAppDbContext : DbContext, IShippingAppDbContext  
    {
        private readonly ICurrentUserService _currentUserService;

        public ShippingAppDbContext(DbContextOptions<ShippingAppDbContext> options, ICurrentUserService currentUserServic) : base(options)
        {
            _currentUserService = currentUserServic ?? throw new ArgumentNullException(nameof(currentUserServic));
        }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShippingPlan> ShippingPlans { get; set; }
        public DbSet<MovementRequest> MovementRequests { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<ReceivedMark> ReceivedMarks { get; set; }
        public DbSet<ShippingMark> ShippingMarks { get; set; }
        public DbSet<ShippingRequest> ShippingRequests { get; set; }

        public DbSet<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
                        entry.Entity.CreatedBy = _currentUserService.UserName;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = _currentUserService.UserName;
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
                        entry.Entity.CreatedBy = _currentUserService.UserName;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = _currentUserService.UserName;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

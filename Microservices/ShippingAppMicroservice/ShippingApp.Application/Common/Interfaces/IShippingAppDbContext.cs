using Microsoft.EntityFrameworkCore;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ShippingApp.Application.Interfaces
{
    public interface IShippingAppDbContext
    {
        DbSet<Entities.ProductType> ProductTypes { get; }
        DbSet<Entities.Country> Countries { get; }
        DbSet<Entities.Product> Products { get; }
        DbSet<Entities.ShippingPlan> ShippingPlans { get; }
        DbSet<TEntity> SetEntity<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken =  new CancellationToken());
        EntityEntry<T> EntryEntity<T>(T entity) where T : class;
    }
}

using Microsoft.EntityFrameworkCore;
//using Entities = Communication.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Communication.Application.Interfaces
{
    public interface ICommunicationDbContext
    {
        DbSet<TEntity> SetEntity<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken =  new CancellationToken());
        EntityEntry<T> EntryEntity<T>(T entity) where T : class;
    }
}

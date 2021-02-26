using System.Threading;
using System.Threading.Tasks;

namespace Communication.Application.Interfaces
{
    public interface ICommunicationDbContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken =  new CancellationToken());
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Communication.Application.Common.Results;

namespace Communication.Application.Interfaces
{
    public interface ICommunicationRepository<T> where T : class
    {
        Task<Result> AddAsync(T entity);
        Task<Result> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<Result> Update(int id, T model);
        DbSet<T> GetDbSet();
    }
}

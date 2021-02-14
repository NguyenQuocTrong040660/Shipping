using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.Interfaces
{
    public interface IShippingAppRepository<T> where T : class
    {
        Task<Result> AddAsync(T entity);
        Task<Result> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<Result> Update(int id, T model);
        DbSet<T> GetDbSet();
    }
}

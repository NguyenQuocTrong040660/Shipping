using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.Interfaces
{
    public interface IShippingAppRepository<T> where T : class
    {
        Task<Result> AddAsync(T entity);
        Task<Result> DeleteAsync(Guid id);
        Task<T> GetAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<Result> Update(Guid id, T model);
    }
}

using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Common.Results;
using System.Linq;

namespace ShippingApp.Infrastructure.Repositories
{
    public class ShippingAppRepository<T> : IShippingAppRepository<T> where T: class
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<T> _dbset;

        public ShippingAppRepository(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbset = _context.SetEntity<T>();
        }

        public async Task<Result> AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0
                ? Result.Success() 
                : Result.Failure($"Failed to add {typeof(T).Name} entity to database");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var entity = await _dbset.FindAsync(id);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbset.Remove(entity);
            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure($"Failed to delete {typeof(T).Name} entity");
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbset.AsNoTracking().ToListAsync();
        }

        public DbSet<T> GetDbSet()
        {
            return _dbset;
        }

        public async Task<Result> Update(int id, T model)
        {
            var entity = await _dbset.FindAsync(id);
            
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.EntryEntity(entity).CurrentValues.SetValues(model);

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure($"Failed to update {typeof(T).Name} entity");
        }
    }
}


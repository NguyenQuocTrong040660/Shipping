using Microsoft.EntityFrameworkCore;
using Entities = ShippingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Domain.Interfaces
{
    public interface IShippingAppDbContext
    {
        DbSet<Entities.ProductType> ProductType { get; set; }
        DbSet<Entities.Country> Country { get; set; }
        DbSet<Entities.ProductEntity> Product { get; set; }
        public int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using ShippingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Domain.Interfaces
{
    public interface IShippingAppDbContext
    {
        DbSet<ProductType> ProductType { get; set; }
        DbSet<ProductDescription> ProductDescription { get; set; }
        DbSet<ProductImage> ProductImage { get; set; }
        DbSet<ProductGeneral> ProductGeneral { get; set; }
        DbSet<ProductSpecification> ProductSpecification { get; set; }
        DbSet<ProductOverview> ProductOverview { get; set; }
        DbSet<Country> Country { get; set; }
        DbSet<Brand> Brand { get; set; }

        DbSet<Customer> Customers { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> orderDetails { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        DbSet<MemberShip> MemberShips { get; set; }
        DbSet<Promotion> Promotions { get; set; }
        public int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

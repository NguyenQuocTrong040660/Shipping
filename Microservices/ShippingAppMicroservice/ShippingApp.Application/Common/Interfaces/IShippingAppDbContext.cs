﻿using Microsoft.EntityFrameworkCore;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ShippingApp.Application.Interfaces
{
    public interface IShippingAppDbContext
    {
        DbSet<Entities.Country> Countries { get; }
        DbSet<Entities.Product> Products { get; }
        DbSet<Entities.ShippingPlan> ShippingPlans { get; }
        DbSet<Entities.ShippingPlanDetail> ShippingPlanDetails { get; }
        DbSet<Entities.MovementRequest> MovementRequests { get; }
        DbSet<Entities.MovementRequestDetail> MovementRequestDetails { get; }
        DbSet<Entities.WorkOrder> WorkOrders { get; }
        DbSet<Entities.WorkOrderDetail> WorkOrderDetails { get; }
        DbSet<Entities.Config> Configs { get; }
        DbSet<Entities.ReceivedMark> ReceivedMarks { get; }
        DbSet<Entities.ShippingMark> ShippingMarks { get; }
        DbSet<Entities.ShippingRequest> ShippingRequests { get; }
        DbSet<Entities.ShippingRequestDetail> ShippingRequestDetails { get; }
        DbSet<Entities.ShippingRequestLogistic> ShippingRequestLogistics { get; }
        DbSet<TEntity> SetEntity<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken =  new CancellationToken());
        EntityEntry<T> EntryEntity<T>(T entity) where T : class;
    }
}
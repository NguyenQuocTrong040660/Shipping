using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class WorkOrderDetailConfiguration : IEntityTypeConfiguration<WorkOrderDetail>
    {
        public void Configure(EntityTypeBuilder<WorkOrderDetail> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasKey(sc => new { sc.WorkOrderId, sc.ProductId });
            builder.HasOne<WorkOrder>(sc => sc.WorkOrder).WithMany(s => s.WorkOrderDetails).HasForeignKey(r => r.WorkOrderId);
            builder.HasOne<Product>(sc => sc.Product).WithMany(s => s.WorkOrderDetails).HasForeignKey(r => r.ProductId);
        }
    }
}

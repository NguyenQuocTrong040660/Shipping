using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasOne<MovementRequest>(s => s.MovementRequest)
                    .WithMany(g => g.WorkOrders)
                    .HasForeignKey(s => s.MovementRequestId)
                    .IsRequired(false);

            builder.HasOne<Product>(s => s.Product)
                    .WithMany(g => g.WorkOrders)
                    .HasForeignKey(s => s.ProductId);
        }
    }
}

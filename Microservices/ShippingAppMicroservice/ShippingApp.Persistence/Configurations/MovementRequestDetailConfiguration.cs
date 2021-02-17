using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class MovementRequestDetailConfiguration : IEntityTypeConfiguration<MovementRequestDetail>
    {
        public void Configure(EntityTypeBuilder<MovementRequestDetail> builder)
        {
            builder.HasKey(sc => new { sc.WorkOrderId, sc.MovementRequestId });
            builder.HasOne<WorkOrder>(sc => sc.WorkOrder).WithMany(s => s.MovementRequestDetails)
                .HasForeignKey(r => r.WorkOrderId);
            builder.HasOne<MovementRequest>(sc => sc.MovementRequest).WithMany(s => s.MovementRequestDetails)
                .HasForeignKey(r => r.MovementRequestId);
        }
    }
}

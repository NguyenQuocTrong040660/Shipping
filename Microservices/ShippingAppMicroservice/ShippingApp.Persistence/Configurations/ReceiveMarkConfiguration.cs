using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ReceiveMarkConfiguration : IEntityTypeConfiguration<ReceivedMark>
    {
        public void Configure(EntityTypeBuilder<ReceivedMark> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasOne<WorkOrder>(s => s.WorkOrder)
                    .WithMany(g => g.ReceiveMarks)
                    .HasForeignKey(s => s.WorkOrderId);
        }
    }
}

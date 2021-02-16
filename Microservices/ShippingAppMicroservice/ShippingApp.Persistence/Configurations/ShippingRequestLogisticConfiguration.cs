using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingRequestLogisticConfiguration : IEntityTypeConfiguration<ShippingRequestLogistic>
    {
        public void Configure(EntityTypeBuilder<ShippingRequestLogistic> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasOne<ShippingRequest>(s => s.ShippingRequest)
                   .WithMany(g => g.ShippingRequestLogistics)
                   .HasForeignKey(s => s.ShippingRequestId);
        }
    }
}

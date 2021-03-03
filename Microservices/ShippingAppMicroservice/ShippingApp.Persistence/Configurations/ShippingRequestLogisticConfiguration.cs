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
        }
    }
}

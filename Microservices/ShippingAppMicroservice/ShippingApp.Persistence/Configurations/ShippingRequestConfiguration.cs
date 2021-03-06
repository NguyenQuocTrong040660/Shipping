using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingRequestConfiguration : IEntityTypeConfiguration<ShippingRequest>
    {
        public void Configure(EntityTypeBuilder<ShippingRequest> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Prefix).HasDefaultValue(PrefixTable.ShippingRequest);
            builder.Property(t => t.Status).HasDefaultValue(ShippingRequestStatus.New);
        }
    }
}

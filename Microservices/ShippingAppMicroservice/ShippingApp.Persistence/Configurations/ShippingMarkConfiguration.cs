using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingMarkConfiguration : IEntityTypeConfiguration<ShippingMark>
    {
        public void Configure(EntityTypeBuilder<ShippingMark> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Prefix).HasDefaultValue(PrefixTable.ShippingMark);
        }
    }
}

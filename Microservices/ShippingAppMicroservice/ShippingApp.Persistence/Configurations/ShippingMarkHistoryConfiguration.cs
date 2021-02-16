using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingMarkHistoryConfiguration : IEntityTypeConfiguration<ShippingMarkHistory>
    {
        public void Configure(EntityTypeBuilder<ShippingMarkHistory> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
        }
    }
}

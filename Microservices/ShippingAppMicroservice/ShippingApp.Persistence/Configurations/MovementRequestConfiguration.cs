using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class MovementRequestConfiguration : IEntityTypeConfiguration<MovementRequest>
    {
        public void Configure(EntityTypeBuilder<MovementRequest> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
        }
    }
}

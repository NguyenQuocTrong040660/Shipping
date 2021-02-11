using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingRequestConfiguration : IEntityTypeConfiguration<ShippingRequest>
    {
        public void Configure(EntityTypeBuilder<ShippingRequest> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasOne<Product>(s => s.Product)
                   .WithMany(g => g.ShippingRequests)
                   .HasForeignKey(s => s.ProductId);
        }
    }
}

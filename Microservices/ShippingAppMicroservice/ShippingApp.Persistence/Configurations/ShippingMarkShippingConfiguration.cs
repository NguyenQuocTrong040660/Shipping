using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingMarkShippingConfiguration : IEntityTypeConfiguration<ShippingMarkShipping>
    {
        public void Configure(EntityTypeBuilder<ShippingMarkShipping> builder)
        {
            builder.HasKey(sc => new { sc.ShippingMarkId, sc.ShippingRequestId, sc.ProductId });

            builder.HasOne<ShippingMark>(sc => sc.ShippingMark)
                .WithMany(s => s.ShippingMarkShippings)
                .HasForeignKey(r => r.ShippingMarkId);

            builder.HasOne<ShippingRequest>(sc => sc.ShippingRequest)
                .WithMany(s => s.ShippingMarkShippings)
                .HasForeignKey(r => r.ShippingRequestId);

            builder.HasOne<Product>(sc => sc.Product)
                .WithMany(s => s.ShippingMarkShippings)
                .HasForeignKey(r => r.ProductId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingMarkSummaryConfiguration : IEntityTypeConfiguration<ShippingMarkSummary>
    {
        public void Configure(EntityTypeBuilder<ShippingMarkSummary> builder)
        {
            builder.HasKey(sc => new { sc.ShippingMarkId, sc.ProductId });

            builder.HasOne<Product>(s => s.Product)
                    .WithMany(g => g.ShippingMarkSummaries)
                    .HasForeignKey(s => s.ProductId);

            builder.HasOne<ShippingMark>(s => s.ShippingMark)
               .WithMany(g => g.ShippingMarkSummaries)
               .HasForeignKey(s => s.ShippingMarkId);
        }
    }
}

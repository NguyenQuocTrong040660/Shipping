using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingMarkConfiguration : IEntityTypeConfiguration<ShippingMark>
    {
        public void Configure(EntityTypeBuilder<ShippingMark> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasOne<Product>(s => s.Product)
                   .WithMany(g => g.ShippingMarks)
                   .HasForeignKey(s => s.ProductId);
        }
    }
}

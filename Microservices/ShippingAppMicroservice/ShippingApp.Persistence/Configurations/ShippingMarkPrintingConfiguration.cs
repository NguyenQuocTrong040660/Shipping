using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingMarkPrintingConfiguration : IEntityTypeConfiguration<ShippingMarkPrinting>
    {
        public void Configure(EntityTypeBuilder<ShippingMarkPrinting> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.PrintCount).HasDefaultValue(0);
            builder.Property(t => t.Status).HasDefaultValue(nameof(ShippingMarkStatus.New));
            builder.Property(t => t.Prefix).HasDefaultValue(PrefixTable.ShippingMarkPrinting);

            builder.HasOne<Product>(s => s.Product)
                    .WithMany(g => g.ShippingMarkPrintings)
                    .HasForeignKey(s => s.ProductId);

            builder.HasOne<ShippingMark>(s => s.ShippingMark)
               .WithMany(g => g.ShippingMarkPrintings)
               .HasForeignKey(s => s.ShippingMarkId);
        }
    }
}

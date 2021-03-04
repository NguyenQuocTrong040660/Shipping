using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class ReceivedMarkPrintingConfiguration : IEntityTypeConfiguration<ReceivedMarkPrinting>
    {
        public void Configure(EntityTypeBuilder<ReceivedMarkPrinting> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.PrintCount).HasDefaultValue(0);
            builder.Property(t => t.Status).HasDefaultValue(nameof(ReceivedMarkStatus.New));

            builder.HasOne<Product>(s => s.Product)
                    .WithMany(g => g.ReceivedMarkPrintings)
                    .HasForeignKey(s => s.ProductId);

            builder.HasOne<ReceivedMark>(s => s.ReceivedMark)
               .WithMany(g => g.ReceivedMarkPrintings)
               .HasForeignKey(s => s.ReceivedMarkId);

            builder.HasOne<ShippingMark>(s => s.ShippingMark)
              .WithMany(g => g.ReceivedMarkPrintings)
              .HasForeignKey(s => s.ShippingMarkId)
              .IsRequired(false);
        }
    }
}

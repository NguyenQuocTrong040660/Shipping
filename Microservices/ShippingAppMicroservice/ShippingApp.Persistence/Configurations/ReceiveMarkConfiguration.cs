using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class ReceiveMarkConfiguration : IEntityTypeConfiguration<ReceivedMark>
    {
        public void Configure(EntityTypeBuilder<ReceivedMark> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Prefix).HasDefaultValue(PrefixTable.ReceivedMark);

            builder.Property(t => t.PrintCount).HasDefaultValue(1);

            builder.HasOne<Product>(s => s.Product)
                    .WithMany(g => g.ReceivedMarks)
                    .HasForeignKey(s => s.ProductId);

            builder.HasOne<MovementRequest>(s => s.MovementRequest)
               .WithMany(g => g.ReceivedMarks)
               .HasForeignKey(s => s.MovementRequestId);
        }
    }
}

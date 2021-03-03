using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingMarkReceivedConfiguration : IEntityTypeConfiguration<ShippingMarkReceived>
    {
        public void Configure(EntityTypeBuilder<ShippingMarkReceived> builder)
        {
            builder.HasKey(sc => new { sc.ShippingMarkId, sc.ReceivedMarkId });

            builder.HasOne<ShippingMark>(sc => sc.ShippingMark)
                .WithMany(s => s.ShippingMarkReceiveds)
                .HasForeignKey(r => r.ShippingMarkId);

            builder.HasOne<ReceivedMark>(sc => sc.ReceivedMark)
                .WithMany(s => s.ShippingMarkReceiveds)
                .HasForeignKey(r => r.ReceivedMarkId);
        }
    }
}

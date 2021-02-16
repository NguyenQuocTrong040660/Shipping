using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ReceiveMarkConfiguration : IEntityTypeConfiguration<ReceivedMark>
    {
        public void Configure(EntityTypeBuilder<ReceivedMark> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.HasOne<Product>(s => s.Product)
                    .WithMany(g => g.ReceivedMarks)
                    .HasForeignKey(s => s.ProductId);
        }
    }
}

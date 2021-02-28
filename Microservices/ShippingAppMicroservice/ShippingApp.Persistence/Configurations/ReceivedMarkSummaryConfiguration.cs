using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class ReceivedMarkSummaryConfiguration : IEntityTypeConfiguration<ReceivedMarkSummary>
    {
        public void Configure(EntityTypeBuilder<ReceivedMarkSummary> builder)
        {
            builder.HasKey(sc => new { sc.ReceivedMarkId, sc.ProductId });

            builder.HasOne<Product>(s => s.Product)
                    .WithMany(g => g.ReceivedMarkSummaries)
                    .HasForeignKey(s => s.ProductId);

            builder.HasOne<ReceivedMark>(s => s.ReceivedMark)
               .WithMany(g => g.ReceivedMarkSummaries)
               .HasForeignKey(s => s.ReceivedMarkId);
        }
    }
}

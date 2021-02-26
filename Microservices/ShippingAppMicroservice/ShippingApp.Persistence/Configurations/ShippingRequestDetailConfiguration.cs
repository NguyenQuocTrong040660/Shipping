using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingRequestDetailConfiguration : IEntityTypeConfiguration<ShippingRequestDetail>
    {
        public void Configure(EntityTypeBuilder<ShippingRequestDetail> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasOne<Product>(s => s.Product)
                   .WithMany(g => g.ShippingRequestDetails)
                   .HasForeignKey(s => s.ProductId);

            builder.HasOne<ShippingRequest>(s => s.ShippingRequest)
                 .WithMany(g => g.ShippingRequestDetails)
                 .HasForeignKey(s => s.ShippingRequestId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingPlanDetailConfiguration : IEntityTypeConfiguration<ShippingPlanDetail>
    {
        public void Configure(EntityTypeBuilder<ShippingPlanDetail> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.HasOne<Product>(s => s.Product)
                   .WithMany(g => g.ShippingPlanDetails)
                   .HasForeignKey(s => s.ProductId);

            builder.HasOne<ShippingPlan>(s => s.ShippingPlan)
                 .WithMany(g => g.ShippingPlanDetails)
                 .HasForeignKey(s => s.ShippingPlanId);
        }
    }
}

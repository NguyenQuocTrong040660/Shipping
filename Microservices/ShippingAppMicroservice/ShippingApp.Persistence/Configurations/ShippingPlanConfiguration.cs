using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingPlanConfiguration : IEntityTypeConfiguration<ShippingPlan>
    {
        public void Configure(EntityTypeBuilder<ShippingPlan> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Prefix).HasDefaultValue(PrefixTable.ShippingPlan);

            builder.HasOne<Product>(s => s.Product)
                   .WithMany(g => g.ShippingPlans)
                   .HasForeignKey(s => s.ProductId);

            builder.HasOne<ShippingRequest>(s => s.ShippingRequest)
                  .WithMany(g => g.ShippingPlans)
                  .HasForeignKey(s => s.ShippingRequestId)
                  .IsRequired(false);
        }
    }
}

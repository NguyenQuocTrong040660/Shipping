using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ShippingPlanConfiguration : IEntityTypeConfiguration<ShippingPlan>
    {
        public void Configure(EntityTypeBuilder<ShippingPlan> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
        }
    }
}

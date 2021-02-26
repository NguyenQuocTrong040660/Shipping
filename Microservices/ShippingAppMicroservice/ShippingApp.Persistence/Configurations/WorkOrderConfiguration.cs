using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Persistence.Configurations
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Prefix).HasDefaultValue(PrefixTable.WorkOrder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingApp.Domain.Entities;

namespace ShippingApp.Persistence.Configurations
{
    public class ReceivedMarkMovementConfiguration : IEntityTypeConfiguration<ReceivedMarkMovement>
    {
        public void Configure(EntityTypeBuilder<ReceivedMarkMovement> builder)
        {
            builder.HasKey(sc => new { sc.ReceivedMarkId, sc.MovementRequestId, sc.ProductId });

            builder.HasOne<ReceivedMark>(sc => sc.ReceivedMark).WithMany(s => s.ReceivedMarkMovements)
                .HasForeignKey(r => r.ReceivedMarkId);

            builder.HasOne<MovementRequest>(sc => sc.MovementRequest).WithMany(s => s.ReceivedMarkMovements)
                .HasForeignKey(r => r.MovementRequestId);

            builder.HasOne<Product>(sc => sc.Product).WithMany(s => s.ReceivedMarkMovements)
               .HasForeignKey(r => r.ProductId);
        }
    }
}

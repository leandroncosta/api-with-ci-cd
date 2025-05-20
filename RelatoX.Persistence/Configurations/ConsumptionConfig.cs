using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RelatoX.Domain.Entities;

namespace RelatoX.Persistence.Configurations
{
    public class ConsumptionConfig : IEntityTypeConfiguration<ConsumptionEntry>

    {
        public void Configure(EntityTypeBuilder<ConsumptionEntry> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id)
         .IsRequired();
            builder.Property(x => x.UserId)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(x => x.QuantityConsumed)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.Date)
          .IsRequired();
        }
    }
}
using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class PaymentStateConfiguration : IEntityTypeConfiguration<PaymentState>
{
    public void Configure(EntityTypeBuilder<PaymentState> builder)
    {
        builder.ToTable("payment_states");
        
        builder.HasKey(ps => ps.Id);
        builder.Property(ps => ps.Id).HasColumnName("id");
        
        builder.Property(ps => ps.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();
            
        builder.HasIndex(ps => ps.Name).IsUnique();

        builder.Property(ps => ps.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(ps => ps.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
using Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Reservations;

public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.ReservationCode).HasColumnName("reservation_code").HasMaxLength(30).IsRequired();
        builder.Property(r => r.ClientId).HasColumnName("client_id");
        builder.Property(r => r.ReservationDate).HasColumnName("reservation_date");
        builder.Property(r => r.ReservationStatusId).HasColumnName("reservation_status_id");
        builder.Property(r => r.TotalValue).HasColumnName("total_value").HasPrecision(18, 2);
        builder.Property(r => r.ExpiresAt).HasColumnName("expires_at");
        builder.Property(r => r.CreatedAt).HasColumnName("created_at");
        builder.Property(r => r.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(r => r.ReservationCode).IsUnique();
        builder.HasOne(r => r.Client)
            .WithMany()
            .HasForeignKey(r => r.ClientId);
        builder.HasOne(r => r.ReservationStatus)
            .WithMany(rs => rs.Reservations)
            .HasForeignKey(r => r.ReservationStatusId);
    }
}
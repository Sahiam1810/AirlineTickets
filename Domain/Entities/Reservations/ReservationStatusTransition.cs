using System;

namespace Domain.Entities.Reservations;
public sealed class ReservationStatusTransition
{
    public int Id { get; set; }
    public int OriginStatusId { get; set; }
    public int DestinationStatusId { get; set; }

    // Navigation
    public ReservationStatus OriginStatus { get; set; } = null!;
    public ReservationStatus DestinationStatus { get; set; } = null!;
}
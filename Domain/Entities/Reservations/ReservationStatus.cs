using System;

namespace Domain.Entities.Reservations;

public sealed class ReservationStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<ReservationStatusTransition> OriginTransitions { get; set; } = [];
    public ICollection<ReservationStatusTransition> DestinationTransitions { get; set; } = [];
    public ICollection<Reservation> Reservations { get; set; } = [];
}
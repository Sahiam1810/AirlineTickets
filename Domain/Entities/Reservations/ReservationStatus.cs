using System;
using Domain.Common;

namespace Domain.Entities.Reservations;

public sealed class ReservationStatus : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<ReservationStatusTransition> OriginTransitions { get; set; } = [];
    public ICollection<ReservationStatusTransition> DestinationTransitions { get; set; } = [];
    public ICollection<Reservation> Reservations { get; set; } = [];
}
using System;
using Domain.Common;
using Domain.ValueObjects.ReservationStatuses;

namespace Domain.Entities.Reservations;

public sealed class ReservationStatus : BaseEntity<int>
{
    public ReservationStatusName Name { get; private set; } = null!;

    private ReservationStatus() { }

    public ReservationStatus(string name)
    {
        Name = ReservationStatusName.Create(name);
    }

    public void Update(string name)
    {
        Name = ReservationStatusName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<ReservationStatusTransition> OriginTransitions { get; set; } = [];
    public ICollection<ReservationStatusTransition> DestinationTransitions { get; set; } = [];
    public ICollection<Reservation> Reservations { get; set; } = [];
}

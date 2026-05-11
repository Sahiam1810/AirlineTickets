using System;
using Domain.Common;

namespace Domain.Entities.Reservations;
public sealed class ReservationStatusTransition : BaseEntity<int>
{
    public int OriginStatusId { get; set; }
    public int DestinationStatusId { get; set; }

    // Navigation
    public ReservationStatus OriginStatus { get; set; } = null!;
    public ReservationStatus DestinationStatus { get; set; } = null!;
}
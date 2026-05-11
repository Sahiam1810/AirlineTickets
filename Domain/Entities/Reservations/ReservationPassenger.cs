using System;
using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Reservations;

public sealed class ReservationPassenger : BaseEntity<int>
{
    public int ReservationFlightId { get; set; }
    public int PassengerId { get; set; }

    // Navigation
    public ReservationFlight ReservationFlight { get; set; } = null!;
    public Passenger Passenger { get; set; } = null!;
}
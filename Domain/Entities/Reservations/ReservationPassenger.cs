using System;
using Domain.Entities.People;

namespace Domain.Entities.Reservations;

public sealed class ReservationPassenger
{
    public int Id { get; set; }
    public int ReservationFlightId { get; set; }
    public int PassengerId { get; set; }

    // Navigation
    public ReservationFlight ReservationFlight { get; set; } = null!;
    public Passenger Passenger { get; set; } = null!;
}
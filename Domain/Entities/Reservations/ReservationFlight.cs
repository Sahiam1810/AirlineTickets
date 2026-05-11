using System;
using Domain.Entities.Flights;

namespace Domain.Entities.Reservations;

public sealed class ReservationFlight
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int FlightId { get; set; }
    public decimal PartialValue { get; set; }

    // Navigation
    public Reservation Reservation { get; set; } = null!;
    public Flight Flight { get; set; } = null!;
    public ICollection<ReservationPassenger> ReservationPassengers { get; set; } = [];
}
using System;
using Domain.Common;
using Domain.Entities.Flights;

namespace Domain.Entities.Reservations;

public sealed class ReservationFlight : BaseEntity<int>
{
    public int ReservationId { get; private set; }
    public int FlightId { get; private set; }
    public decimal PartialValue { get; private set; }

    private ReservationFlight() { }

    public ReservationFlight(int reservationId, int flightId, decimal partialValue)
    {
        Validate(reservationId, flightId, partialValue);

        ReservationId = reservationId;
        FlightId = flightId;
        PartialValue = partialValue;
    }

    public void Update(int reservationId, int flightId, decimal partialValue)
    {
        Validate(reservationId, flightId, partialValue);

        ReservationId = reservationId;
        FlightId = flightId;
        PartialValue = partialValue;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int reservationId, int flightId, decimal partialValue)
    {
        if (reservationId <= 0)
        {
            throw new ArgumentException("Reservation id must be greater than 0.", nameof(reservationId));
        }

        if (flightId <= 0)
        {
            throw new ArgumentException("Flight id must be greater than 0.", nameof(flightId));
        }

        if (partialValue < 0)
        {
            throw new ArgumentException("Partial value cannot be negative.", nameof(partialValue));
        }
    }

    public Reservation Reservation { get; set; } = null!;
    public Flight Flight { get; set; } = null!;
    public ICollection<ReservationPassenger> ReservationPassengers { get; set; } = [];
}

using System;
using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Reservations;

public sealed class ReservationPassenger : BaseEntity<int>
{
    public int ReservationFlightId { get; private set; }
    public int PassengerId { get; private set; }

    private ReservationPassenger() { }

    public ReservationPassenger(int reservationFlightId, int passengerId)
    {
        Validate(reservationFlightId, passengerId);

        ReservationFlightId = reservationFlightId;
        PassengerId = passengerId;
    }

    public void Update(int reservationFlightId, int passengerId)
    {
        Validate(reservationFlightId, passengerId);

        ReservationFlightId = reservationFlightId;
        PassengerId = passengerId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int reservationFlightId, int passengerId)
    {
        if (reservationFlightId <= 0)
        {
            throw new ArgumentException("Reservation flight id must be greater than 0.", nameof(reservationFlightId));
        }

        if (passengerId <= 0)
        {
            throw new ArgumentException("Passenger id must be greater than 0.", nameof(passengerId));
        }
    }

    public ReservationFlight ReservationFlight { get; set; } = null!;
    public Passenger Passenger { get; set; } = null!;
}

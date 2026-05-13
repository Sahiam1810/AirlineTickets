namespace Api.Dtos.ReservationPassengers;

public sealed class UpdateReservationPassengerRequest
{
    public int ReservationFlightId { get; init; }
    public int PassengerId { get; init; }
}

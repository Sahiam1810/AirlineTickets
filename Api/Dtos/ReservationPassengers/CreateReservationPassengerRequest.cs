namespace Api.Dtos.ReservationPassengers;

public sealed class CreateReservationPassengerRequest
{
    public int ReservationFlightId { get; init; }
    public int PassengerId { get; init; }
}

namespace Api.Dtos.ReservationFlights;

public sealed class CreateReservationFlightRequest
{
    public int ReservationId { get; init; }
    public int FlightId { get; init; }
    public decimal PartialValue { get; init; }
}

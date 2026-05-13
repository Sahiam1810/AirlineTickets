namespace Api.Dtos.ReservationFlights;

public sealed class ReservationFlightDto
{
    public int Id { get; init; }
    public int ReservationId { get; init; }
    public string ReservationCode { get; init; } = string.Empty;
    public int FlightId { get; init; }
    public string FlightCode { get; init; } = string.Empty;
    public decimal PartialValue { get; init; }
    public DateTime CreatedAt { get; init; }
}

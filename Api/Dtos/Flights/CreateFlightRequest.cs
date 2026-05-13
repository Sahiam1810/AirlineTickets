namespace Api.Dtos.Flights;

public sealed class CreateFlightRequest
{
    public string FlightCode { get; init; } = default!;
    public int AirlineId { get; init; }
    public int RouteId { get; init; }
    public int AircraftId { get; init; }
    public DateTime DepartureDate { get; init; }
    public DateTime EstimatedArrivalDate { get; init; }
    public int TotalCapacity { get; init; }
    public int AvailableSeats { get; init; }
    public int FlightStateId { get; init; }
    public DateTime? RescheduledAt { get; init; }
}

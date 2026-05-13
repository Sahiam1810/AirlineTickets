namespace Api.Dtos.Flights;

public sealed class FlightDto
{
    public int Id { get; init; }
    public string FlightCode { get; init; } = default!;
    public int AirlineId { get; init; }
    public string AirlineName { get; init; } = default!;
    public string AirlineIataCode { get; init; } = default!;
    public int RouteId { get; init; }
    public int OriginAirportId { get; init; }
    public string OriginAirportName { get; init; } = default!;
    public string OriginAirportIataCode { get; init; } = default!;
    public int DestinationAirportId { get; init; }
    public string DestinationAirportName { get; init; } = default!;
    public string DestinationAirportIataCode { get; init; } = default!;
    public int AircraftId { get; init; }
    public string AircraftRegistration { get; init; } = default!;
    public string AircraftModelName { get; init; } = default!;
    public DateTime DepartureDate { get; init; }
    public DateTime EstimatedArrivalDate { get; init; }
    public int TotalCapacity { get; init; }
    public int AvailableSeats { get; init; }
    public int FlightStateId { get; init; }
    public string FlightStateName { get; init; } = default!;
    public DateTime? RescheduledAt { get; init; }
}

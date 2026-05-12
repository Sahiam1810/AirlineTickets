namespace Api.Dtos.Routes;

public sealed class RouteDto
{
    public int Id { get; init; }
    public int OriginAirportId { get; init; }
    public string OriginAirportName { get; init; } = default!;
    public string OriginAirportIataCode { get; init; } = default!;
    public int DestinationAirportId { get; init; }
    public string DestinationAirportName { get; init; } = default!;
    public string DestinationAirportIataCode { get; init; } = default!;
    public int? DistanceKm { get; init; }
    public int? EstimatedDurationMinutes { get; init; }
}

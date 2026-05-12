namespace Api.Dtos.RouteStops;

public sealed class RouteStopDto
{
    public int Id { get; init; }
    public int RouteId { get; init; }
    public int OriginAirportId { get; init; }
    public string OriginAirportName { get; init; } = default!;
    public string OriginAirportIataCode { get; init; } = default!;
    public int DestinationAirportId { get; init; }
    public string DestinationAirportName { get; init; } = default!;
    public string DestinationAirportIataCode { get; init; } = default!;
    public int StopAirportId { get; init; }
    public string StopAirportName { get; init; } = default!;
    public string StopAirportIataCode { get; init; } = default!;
    public int Order { get; init; }
    public int StopDurationMinutes { get; init; }
}

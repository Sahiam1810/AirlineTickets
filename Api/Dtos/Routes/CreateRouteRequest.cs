namespace Api.Dtos.Routes;

public sealed class CreateRouteRequest
{
    public int OriginAirportId { get; init; }
    public int DestinationAirportId { get; init; }
    public int? DistanceKm { get; init; }
    public int? EstimatedDurationMinutes { get; init; }
}

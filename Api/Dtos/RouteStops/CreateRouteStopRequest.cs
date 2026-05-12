namespace Api.Dtos.RouteStops;

public sealed class CreateRouteStopRequest
{
    public int RouteId { get; init; }
    public int StopAirportId { get; init; }
    public int Order { get; init; }
    public int StopDurationMinutes { get; init; }
}

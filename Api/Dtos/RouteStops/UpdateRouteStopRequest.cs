namespace Api.Dtos.RouteStops;

public sealed class UpdateRouteStopRequest
{
    public int StopAirportId { get; init; }
    public int Order { get; init; }
    public int StopDurationMinutes { get; init; }
}

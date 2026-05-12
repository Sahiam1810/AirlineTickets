using System;
using Domain.Common;
using Domain.Entities.Airlines;
using Domain.ValueObjects.Routes;

namespace Domain.Entities.Routes;

public sealed class RouteStop : BaseEntity<int>
{
    public int RouteId { get; private set; }
    public int StopAirportId { get; private set; }
    public StopOrder Order { get; private set; } = null!;
    public StopDurationMinutes StopDurationMinutes { get; private set; } = null!;

    private RouteStop() { }

    public RouteStop(int routeId, int stopAirportId, StopOrder order, StopDurationMinutes stopDurationMinutes)
    {
        Validate(routeId, stopAirportId);

        RouteId = routeId;
        StopAirportId = stopAirportId;
        Order = order;
        StopDurationMinutes = stopDurationMinutes;
    }

    public void Update(int stopAirportId, StopOrder order, StopDurationMinutes stopDurationMinutes)
    {
        if (stopAirportId <= 0)
            throw new ArgumentException("Stop airport id is required", nameof(stopAirportId));

        StopAirportId = stopAirportId;
        Order = order;
        StopDurationMinutes = stopDurationMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int routeId, int stopAirportId)
    {
        if (routeId <= 0)
            throw new ArgumentException("Route id is required", nameof(routeId));

        if (stopAirportId <= 0)
            throw new ArgumentException("Stop airport id is required", nameof(stopAirportId));
    }

    // Navigation
    public Route Route { get; set; } = null!;
    public Airport StopAirport { get; set; } = null!;
}

using System;
using Domain.Common;
using Domain.Entities.Airlines;

namespace Domain.Entities.Routes;

public sealed class RouteStop : BaseEntity<int>
{
    public int RouteId { get; set; }
    public int StopAirportId { get; set; }
    public int Order { get; set; }
    public int StopDurationMin { get; set; }

    // Navigation
    public Route Route { get; set; } = null!;
    public Airport StopAirport { get; set; } = null!;
}
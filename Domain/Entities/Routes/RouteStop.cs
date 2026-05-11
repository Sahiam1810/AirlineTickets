using System;
using Domain.Entities.Airlines;

namespace Domain.Entities.Routes;

public sealed class RouteStop
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int StopAirportId { get; set; }
    public int Order { get; set; }
    public int StopDurationMin { get; set; }

    // Navigation
    public Route Route { get; set; } = null!;
    public Airport StopAirport { get; set; } = null!;
}
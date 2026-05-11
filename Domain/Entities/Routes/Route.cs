using System;
using Domain.Common;
using Domain.Entities.Airlines;

namespace Domain.Entities.Routes;

public sealed class Route : BaseEntity<int>
{
    public int OriginAirportId { get; set; }
    public int DestinationAirportId { get; set; }
    public int? DistanceKm { get; set; }
    public int? EstimatedDurationMin { get; set; }

    // Navigation
    public Airport OriginAirport { get; set; } = null!;
    public Airport DestinationAirport { get; set; } = null!;
    public ICollection<RouteStop> RouteStops { get; set; } = [];
}
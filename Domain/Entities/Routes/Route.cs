using System;
using Domain.Common;
using Domain.Entities.Airlines;
using Domain.ValueObjects.Routes;

namespace Domain.Entities.Routes;

public sealed class Route : BaseEntity<int>
{
    public int OriginAirportId { get; private set; }
    public int DestinationAirportId { get; private set; }
    public DistanceKm? DistanceKm { get; private set; }
    public DurationMinutes? EstimatedDurationMinutes { get; private set; }

    private Route() { }

    public Route(
        int originAirportId,
        int destinationAirportId,
        DistanceKm? distanceKm,
        DurationMinutes? estimatedDurationMinutes)
    {
        Validate(originAirportId, destinationAirportId);

        OriginAirportId = originAirportId;
        DestinationAirportId = destinationAirportId;
        DistanceKm = distanceKm;
        EstimatedDurationMinutes = estimatedDurationMinutes;
    }

    public void Update(
        int originAirportId,
        int destinationAirportId,
        DistanceKm? distanceKm,
        DurationMinutes? estimatedDurationMinutes)
    {
        Validate(originAirportId, destinationAirportId);

        OriginAirportId = originAirportId;
        DestinationAirportId = destinationAirportId;
        DistanceKm = distanceKm;
        EstimatedDurationMinutes = estimatedDurationMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int originAirportId, int destinationAirportId)
    {
        if (originAirportId <= 0)
            throw new ArgumentException("Origin airport id is required", nameof(originAirportId));

        if (destinationAirportId <= 0)
            throw new ArgumentException("Destination airport id is required", nameof(destinationAirportId));

        if (originAirportId == destinationAirportId)
            throw new ArgumentException("Origin airport and destination airport cannot be the same");
    }

    // Navigation
    public Airport OriginAirport { get; set; } = null!;
    public Airport DestinationAirport { get; set; } = null!;
    public ICollection<RouteStop> RouteStops { get; set; } = [];
}

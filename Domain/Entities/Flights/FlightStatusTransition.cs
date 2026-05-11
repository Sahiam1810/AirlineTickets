using System;

namespace Domain.Entities.Flights;

public sealed class FlightStatusTransition
{
    public int Id { get; set; }
    public int OriginStateId { get; set; }
    public int DestinationStateId { get; set; }

    // Navigation
    public FlightState OriginState { get; set; } = null!;
    public FlightState DestinationState { get; set; } = null!;
}
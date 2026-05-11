using System;
using Domain.Common;

namespace Domain.Entities.Flights;

public sealed class FlightStatusTransition : BaseEntity<int>
{
    public int OriginStateId { get; set; }
    public int DestinationStateId { get; set; }

    // Navigation
    public FlightState OriginState { get; set; } = null!;
    public FlightState DestinationState { get; set; } = null!;
}
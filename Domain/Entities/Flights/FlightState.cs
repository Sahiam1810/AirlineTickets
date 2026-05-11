using System;
using Domain.Common;

namespace Domain.Entities.Flights;

public sealed class FlightState : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<FlightStatusTransition> OriginTransitions { get; set; } = [];
    public ICollection<FlightStatusTransition> DestinationTransitions { get; set; } = [];
    public ICollection<Flight> Flights { get; set; } = [];
}
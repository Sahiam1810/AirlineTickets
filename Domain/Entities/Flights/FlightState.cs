using System;

namespace Domain.Entities.Flights;

public sealed class FlightState
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<FlightStatusTransition> OriginTransitions { get; set; } = [];
    public ICollection<FlightStatusTransition> DestinationTransitions { get; set; } = [];
    public ICollection<Flight> Flights { get; set; } = [];
}
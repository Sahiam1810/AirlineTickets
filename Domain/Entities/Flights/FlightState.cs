using System;
using Domain.Common;
using Domain.ValueObjects.FlightStates;

namespace Domain.Entities.Flights;

public sealed class FlightState : BaseEntity<int>
{
    public FlightStateName Name { get; private set; } = null!;

    private FlightState() { }

    public FlightState(string name)
    {
        Name = FlightStateName.Create(name);
    }

    public void Update(string name)
    {
        Name = FlightStateName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<Flight> Flights { get; set; } = [];
    public ICollection<FlightStatusTransition> FromTransitions { get; set; } = [];
    public ICollection<FlightStatusTransition> ToTransitions { get; set; } = [];
}

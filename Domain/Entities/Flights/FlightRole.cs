using System;

namespace Domain.Entities.Flights;

public sealed class FlightRole
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<FlightAssignment> FlightAssignments { get; set; } = [];
}
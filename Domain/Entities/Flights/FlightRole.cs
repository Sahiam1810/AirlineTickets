using System;
using Domain.Common;

namespace Domain.Entities.Flights;

public sealed class FlightRole : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<FlightAssignment> FlightAssignments { get; set; } = [];
}
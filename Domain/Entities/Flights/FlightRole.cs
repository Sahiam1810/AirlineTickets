using System;
using Domain.Common;
using Domain.ValueObjects.FlightRoles;

namespace Domain.Entities.Flights;

public sealed class FlightRole : BaseEntity<int>
{
    public FlightRoleName Name { get; private set; } = null!;

    private FlightRole() { }

    public FlightRole(string name)
    {
        Name = FlightRoleName.Create(name);
    }

    public void Update(string name)
    {
        Name = FlightRoleName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<FlightAssignment> FlightAssignments { get; set; } = [];
}

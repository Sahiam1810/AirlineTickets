using System;
using Domain.Common;
using Domain.Entities.Staff;
using Domain.ValueObjects.FlightAssignments;

namespace Domain.Entities.Flights;

public sealed class FlightAssignment : BaseEntity<int>
{
    public int FlightId { get; private set; }
    public int StaffId { get; private set; }
    public int FlightRoleId { get; private set; }

    private FlightAssignment() { }

    public FlightAssignment(int flightId, int staffId, int flightRoleId)
    {
        var key = AssignmentKey.Create(flightId, staffId);
        ValidateFlightRoleId(flightRoleId);

        FlightId = key.FlightId;
        StaffId = key.StaffId;
        FlightRoleId = flightRoleId;
    }

    public void Update(int flightId, int staffId, int flightRoleId)
    {
        var key = AssignmentKey.Create(flightId, staffId);
        ValidateFlightRoleId(flightRoleId);

        FlightId = key.FlightId;
        StaffId = key.StaffId;
        FlightRoleId = flightRoleId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateFlightRoleId(int flightRoleId)
    {
        if (flightRoleId <= 0)
            throw new ArgumentException("Flight role id is required", nameof(flightRoleId));
    }

    // Navigation
    public Flight Flight { get; set; } = null!;
    public StaffMember Staff { get; set; } = null!;
    public FlightRole FlightRole { get; set; } = null!;
}

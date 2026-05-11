using System;
using Domain.Common;
using Domain.Entities.Staff;

namespace Domain.Entities.Flights;

public sealed class FlightAssignment : BaseEntity<int>
{
    public int FlightId { get; set; }
    public int StaffMemberId { get; set; }
    public int FlightRoleId { get; set; }

    // Navigation
    public Flight Flight { get; set; } = null!;
    public StaffMember StaffMember { get; set; } = null!;
    public FlightRole FlightRole { get; set; } = null!;
}
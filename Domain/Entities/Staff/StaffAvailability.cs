using System;
using Domain.Common;

namespace Domain.Entities.Staff;

public sealed class StaffAvailability : BaseEntity<int>
{
    public int StaffMemberId { get; set; }
    public int AvailabilityStatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Observation { get; set; }

    // Navigation
    public StaffMember StaffMember { get; set; } = null!;
    public AvailabilityStatus AvailabilityStatus { get; set; } = null!;
}
using System;

namespace Domain.Entities.Staff;

public sealed class StaffAvailability
{
    public int Id { get; set; }
    public int StaffMemberId { get; set; }
    public int AvailabilityStatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Observation { get; set; }

    // Navigation
    public StaffMember StaffMember { get; set; } = null!;
    public AvailabilityStatus AvailabilityStatus { get; set; } = null!;
}
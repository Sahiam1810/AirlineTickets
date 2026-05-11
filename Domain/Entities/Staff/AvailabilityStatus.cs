using System;

namespace Domain.Entities.Staff;

public sealed class AvailabilityStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<StaffAvailability> StaffAvailabilities { get; set; } = [];
}
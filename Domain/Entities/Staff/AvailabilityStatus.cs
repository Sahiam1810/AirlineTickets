using System;
using Domain.Common;

namespace Domain.Entities.Staff;

public sealed class AvailabilityStatus : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<StaffAvailability> StaffAvailabilities { get; set; } = [];
}
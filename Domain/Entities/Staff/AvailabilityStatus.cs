using System;
using Domain.Common;
using Domain.ValueObjects.Staff;

namespace Domain.Entities.Staff;

public sealed class AvailabilityStatus : BaseEntity<int>
{
    public AvailabilityStatusName Name { get; private set; } = null!;

    private AvailabilityStatus() { }

    public AvailabilityStatus(AvailabilityStatusName name)
    {
        Name = name;
    }

    public void Update(AvailabilityStatusName name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<StaffAvailability> StaffAvailabilities { get; set; } = [];
}

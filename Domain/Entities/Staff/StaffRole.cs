using System;
using Domain.Common;
using Domain.ValueObjects.Staff;

namespace Domain.Entities.Staff;

public sealed class StaffRole : BaseEntity<int>
{
    public StaffRoleName Name { get; private set; } = null!;

    private StaffRole() { }

    public StaffRole(StaffRoleName name)
    {
        Name = name;
    }

    public void Update(StaffRoleName name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<StaffMember> StaffMembers { get; set; } = [];
}

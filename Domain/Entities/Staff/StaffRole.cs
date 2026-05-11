using System;
using Domain.Common;

namespace Domain.Entities.Staff;

public sealed class StaffRole : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<StaffMember> StaffMembers { get; set; } = [];
}
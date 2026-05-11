using System;

namespace Domain.Entities.Staff;

public sealed class StaffRole
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<StaffMember> StaffMembers { get; set; } = [];
}
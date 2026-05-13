using System;
using Domain.Common;
using Domain.ValueObjects.Auth;

namespace Domain.Entities.Auth;

public sealed class SystemRole : BaseEntity<int>
{
    public SystemRoleName Name { get; private set; } = null!;
    public string? Description { get; private set; }

    // Navigation
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
    public ICollection<User> Users { get; set; } = [];

    private SystemRole() { }

    public SystemRole(string name, string? description)
    {
        Name = SystemRoleName.Create(name);
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = SystemRoleName.Create(name);
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
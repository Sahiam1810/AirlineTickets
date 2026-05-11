using System;
using Domain.Common;

namespace Domain.Entities.Auth;

public sealed class SystemRole : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    // Navigation
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
    public ICollection<User> Users { get; set; } = [];

    private SystemRole() { }

    public SystemRole(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
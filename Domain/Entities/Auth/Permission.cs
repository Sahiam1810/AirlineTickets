using System;
using Domain.Common;

namespace Domain.Entities.Auth;

public sealed class Permission : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    // Navigation
    public ICollection<RolePermission> RolePermissions { get; set; } = [];

    private Permission() { }

    public Permission(string name, string? description)
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
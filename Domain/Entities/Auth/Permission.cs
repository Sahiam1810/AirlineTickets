using System;
using Domain.Common;
using Domain.ValueObjects.Auth;

namespace Domain.Entities.Auth;

public sealed class Permission : BaseEntity<int>
{
    public PermissionName Name { get; private set; } = null!;
    public string? Description { get; private set; }

    // Navigation
    public ICollection<RolePermission> RolePermissions { get; set; } = [];

    private Permission() { }

    public Permission(string name, string? description)
    {
        Name = PermissionName.Create(name);
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = PermissionName.Create(name);
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
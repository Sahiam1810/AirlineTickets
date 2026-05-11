using System;
using Domain.Common;

namespace Domain.Entities.Auth;

public sealed class RolePermission : BaseEntity<int>
{
    public int RoleId { get; private set; }
    public int PermissionId { get; private set; }

    // Navigation
    public SystemRole SystemRole { get; set; } = null!;
    public Permission Permission { get; set; } = null!;

    private RolePermission() { }

    public RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
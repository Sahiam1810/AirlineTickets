using System;
using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Auth;

public sealed class User : BaseEntity<int>
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public int? PersonId { get; private set; }
    public int RoleId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastAccess { get; private set; }

    // Navigation
    public Person? Person { get; set; }
    public SystemRole SystemRole { get; set; } = null!;
    public ICollection<Session> Sessions { get; set; } = [];

    private User() { }

    public User(string username, string passwordHash, int? personId, int roleId)
    {
        Username = username;
        PasswordHash = passwordHash;
        PersonId = personId;
        RoleId = roleId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string username, int roleId, bool isActive)
    {
        Username = username;
        RoleId = roleId;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordAccess()
    {
        LastAccess = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}
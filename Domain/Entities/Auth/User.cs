using System;
using Domain.Common;
using Domain.Entities.People;
using Domain.ValueObjects.Auth;

namespace Domain.Entities.Auth;

public sealed class User : BaseEntity<int>
{
    public Username Username { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
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
        Username = Username.Create(username);
        PasswordHash = PasswordHash.Create(passwordHash);
        PersonId = personId;
        RoleId = roleId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string username, int? personId, int roleId, bool isActive)
    {
        Username = Username.Create(username);
        PersonId = personId;
        RoleId = roleId;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLastAccess()
    {
        LastAccess = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = PasswordHash.Create(newPasswordHash);
        UpdatedAt = DateTime.UtcNow;
    }
}
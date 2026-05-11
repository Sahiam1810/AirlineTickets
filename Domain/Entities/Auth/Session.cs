using System;
using Domain.Common;

namespace Domain.Entities.Auth;

public sealed class Session : BaseEntity<int>
{
    public int UserId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public string? IpOrigin { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation
    public User User { get; set; } = null!;

    private Session() { }

    public Session(int userId, string? ipOrigin)
    {
        UserId = userId;
        IpOrigin = ipOrigin;
        StartedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Close()
    {
        ClosedAt = DateTime.UtcNow;
        IsActive = false;
    }
}
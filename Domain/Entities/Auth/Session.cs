using System;
using Domain.Common;
using Domain.ValueObjects.Auth;

namespace Domain.Entities.Auth;

public sealed class Session : BaseEntity<int>
{
    public int UserId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public IpAddress? IpAddress { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation
    public User User { get; set; } = null!;

    private Session() { }

    public Session(int userId, string? ipAddress)
    {
        UserId = userId;
        IpAddress = ipAddress is null ? null : IpAddress.Create(ipAddress);
        StartedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void CloseSession()
    {
        EndedAt = DateTime.UtcNow;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
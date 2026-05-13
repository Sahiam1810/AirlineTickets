using System;
using Domain.Common;

namespace Domain.Entities.Tickets;

public sealed class CheckInStatus : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    private CheckInStatus() { }

    public CheckInStatus(string name)
    {
        Name = ValidateName(name);
    }

    public void Update(string name)
    {
        Name = ValidateName(name);
        UpdatedAt = DateTime.UtcNow;
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Check-in status name is required.", nameof(name));
        }

        var normalized = name.Trim();

        if (normalized.Length > 50)
        {
            throw new ArgumentException("Check-in status name cannot exceed 50 characters.", nameof(name));
        }

        return normalized;
    }

    public ICollection<CheckIn> CheckIns { get; set; } = [];
}

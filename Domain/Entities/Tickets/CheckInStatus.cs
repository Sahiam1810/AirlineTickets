using System;
using Domain.Common;

namespace Domain.Entities.Tickets;
public sealed class CheckInStatus : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    // Navigation
    public ICollection<CheckIn> CheckIns { get; set; } = [];

    private CheckInStatus() { }

    public CheckInStatus(string name)
    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
using System;

namespace Domain.Entities.Location;

public sealed class RoadType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<Address> Addresses { get; set; } = [];
}

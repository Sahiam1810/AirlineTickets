using System;

namespace Domain.Entities.Geography;

public sealed class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RegionId { get; set; }

    // Navigation
    public Region Region { get; set; } = null!;
}
using System;
using Domain.Common;

namespace Domain.Entities.Geography;

public sealed class City : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public int RegionId { get; set; }

    // Navigation
    public Region Region { get; set; } = null!;
}
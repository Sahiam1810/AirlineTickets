using System;
using Domain.Common;

namespace Domain.Entities.Geography;

public sealed class Country : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string IsoCode { get; set; } = string.Empty;
    public int ContinentId { get; set; }

    // Navigation
    public Continent Continent { get; set; } = null!;
    public ICollection<Region> Regions { get; set; } = [];
}
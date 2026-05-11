using System;

namespace Domain.Entities.Geography;

public sealed class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IsoCode { get; set; } = string.Empty;
    public int ContinentId { get; set; }

    // Navigation
    public Continent Continent { get; set; } = null!;
    public ICollection<Region> Regions { get; set; } = [];
}
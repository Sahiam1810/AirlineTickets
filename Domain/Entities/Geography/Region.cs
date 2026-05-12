using System;
using Domain.Common;
using Domain.ValueObjects.Geography;

namespace Domain.Entities.Geography;

public sealed class Region : BaseEntity<int>
{
    public RegionName Name { get; private set; } = null!;
    public string Type { get; private set; } = string.Empty;
    public int CountryId { get; private set; }

    // Navigation
    public Country Country { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = [];

    private Region() { }
    public Region(string name, string type, int countryId)
    {
        Name = RegionName.Create(name);
        Type = type;
        CountryId = countryId;
    }
    public void Update(string name, string type)
    {
        Name = RegionName.Create(name);
        Type = type;
        UpdatedAt = DateTime.UtcNow;
    }
}
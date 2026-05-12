using System;
using Domain.Common;
using Domain.ValueObjects.Geography;

namespace Domain.Entities.Geography;

public sealed class Country : BaseEntity<int>
{
    public CountryName Name { get; private set; } = null!;
    public int ContinentId { get; private set; }
    public Continent Continent { get; set; } = null!;
    public ICollection<Region> Regions { get; set; } = [];

    private Country() { }

    public Country(string name, int continentId)
    {
        Name = CountryName.Create(name);
        ContinentId = continentId;
    }

    public void Update(string name, int continentId)
    {
        Name = CountryName.Create(name);
        ContinentId = continentId;
        UpdatedAt = DateTime.UtcNow;
    }
}

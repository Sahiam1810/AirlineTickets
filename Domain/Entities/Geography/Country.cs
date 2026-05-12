using System;
using Domain.Common;
using Domain.ValueObjects.Geography;

namespace Domain.Entities.Geography;

public sealed class Country : BaseEntity<int>
{
    public CountryName Name { get; private set; } = null!;
    public IsoCode IsoCode { get; private set; } = null!;
    public int ContinentId { get; private set; }
    // Navigation
    public Continent Continent { get; set; } = null!;
    public ICollection<Region> Regions { get; set; } = [];

    private Country() { }
    public Country(string name, string isoCode, int continentId)
    {
        Name = CountryName.Create(name);
        IsoCode = IsoCode.Create(isoCode);
        ContinentId = continentId;
    }

    public void Update(string name, string isoCode)
    {
        Name = CountryName.Create(name);
        IsoCode = IsoCode.Create(isoCode);
        UpdatedAt = DateTime.UtcNow;
    }
}
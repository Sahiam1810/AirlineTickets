using System;
using Domain.Common;
using Domain.ValueObjects.Geography;

namespace Domain.Entities.Geography;

public sealed class Region : BaseEntity<int>
{
    public RegionName Name { get; private set; } = null!;
    public RegionType Type { get; private set; } = null!;
    public int CountryId { get; private set; }

    public Country Country { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = [];

    private Region() { }
    public Region(string name, string type, int countryId)
    {
        Name = RegionName.Create(name);
        Type = RegionType.Create(type);
        CountryId = countryId;
    }

    public void Update(string name, string type, int countryId)
    {
        Name = RegionName.Create(name);
        Type = RegionType.Create(type);
        CountryId = countryId;
        UpdatedAt = DateTime.UtcNow;
    }
}

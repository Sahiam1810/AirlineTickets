using Domain.Common;
using Domain.ValueObjects.Geography;

namespace Domain.Entities.Geography;

public sealed class City : BaseEntity<int>
{
    public CityName Name { get; private set; } = null!;
    public int RegionId { get; private set; }

    // Navigation
    public Region Region { get; set; } = null!;

    private City() { }

    public City(string name, int regionId)
    {
        Name = CityName.Create(name);
        RegionId = regionId;
    }

    public void Update(string name)
    {
        Name = CityName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }
}
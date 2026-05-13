using Domain.Common;
using Domain.Entities.Location;
using Domain.ValueObjects.Geography;

namespace Domain.Entities.Geography;

public sealed class City : BaseEntity<int>
{
    public CityName Name { get; private set; } = null!;
    public int RegionId { get; private set; }

    public Region Region { get; set; } = null!;
    public ICollection<Address> Addresses { get; set; } = [];

    private City() { }

    public City(string name, int regionId)
    {
        Name = CityName.Create(name);
        RegionId = regionId;
    }

    public void Update(string name, int regionId)
    {
        Name = CityName.Create(name);
        RegionId = regionId;
        UpdatedAt = DateTime.UtcNow;
    }
}

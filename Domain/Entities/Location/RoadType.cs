using System;
using Domain.Common;
using Domain.ValueObjects.Location;

namespace Domain.Entities.Location;

public sealed class RoadType : BaseEntity<int>
{
    public RoadTypeName Name { get; private set; } = null!;

    public ICollection<Address> Addresses { get; set; } = [];

    private RoadType() { }

    public RoadType(string name)
    {
        Name = RoadTypeName.Create(name);
    }

    public void Update(string name)
    {
        Name = RoadTypeName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }
}

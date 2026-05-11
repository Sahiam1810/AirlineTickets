using System;
using Domain.Common;

namespace Domain.Entities.Location;

public sealed class RoadType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<Address> Addresses { get; set; } = [];
}

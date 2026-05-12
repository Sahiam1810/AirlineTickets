using System;
using Domain.Common;
using Domain.ValueObjects.Aircraft;

namespace Domain.Entities.Aircraft;

public sealed class CabinType : BaseEntity<int>
{
    public CabinTypeName Name { get; private set; } = null!;

    private CabinType() { }

    public CabinType(CabinTypeName name)
    {
        Name = name;
    }

    public void Update(CabinTypeName name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<CabinConfiguration> CabinConfigurations { get; set; } = [];
}

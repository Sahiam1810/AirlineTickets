using System;
using Domain.Common;

namespace Domain.Entities.Aircraft;

public sealed class CabinType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<CabinConfiguration> CabinConfigurations { get; set; } = [];
}
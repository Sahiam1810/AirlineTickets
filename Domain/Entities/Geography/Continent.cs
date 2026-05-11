using System;
using Domain.Common;

namespace Domain.Entities.Geography;

public sealed class Continent : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<Country> Countries { get; set; } = [];
}
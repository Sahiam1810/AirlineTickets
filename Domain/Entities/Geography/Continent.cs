using System;
using Domain.Common;
using Domain.ValueObjects.Continents;

namespace Domain.Entities.Geography;

public sealed class Continent : BaseEntity<int>
{
    public ContinentName Name { get; private set; } = null!;
    private Continent() { }

    public Continent(string name)
    {
        Name = ContinentName.Create(name);;
    }

    public void Updated(string name)
    {
        Name = ContinentName.Create(name);
        UpdatedAt = DateTime.UtcNow;
    }
    public ICollection<Country> Countries { get; set; } = [];
}
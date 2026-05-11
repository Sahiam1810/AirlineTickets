using System;

namespace Domain.Entities.Geography;

public sealed class Continent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<Country> Countries { get; set; } = [];
}
using System;

namespace Domain.Entities.Geography;

public sealed class Region
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int CountryId { get; set; }

    // Navigation
    public Country Country { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = [];
}
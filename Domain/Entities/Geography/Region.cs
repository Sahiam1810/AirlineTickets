using System;
using Domain.Common;

namespace Domain.Entities.Geography;

public sealed class Region : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int CountryId { get; set; }

    // Navigation
    public Country Country { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = [];
}
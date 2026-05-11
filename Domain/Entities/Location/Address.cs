using System;
using Domain.Entities.Geography;

namespace Domain.Entities.Location;

public sealed class Address
{
    public int Id { get; set; }
    public int RoadTypeId { get; set; }
    public string RoadName { get; set; } = string.Empty;
    public string? Number { get; set; }
    public string? Complement { get; set; }
    public int CityId { get; set; }
    public string? PostalCode { get; set; }

    // Navigation
    public RoadType RoadType { get; set; } = null!;
    public City City { get; set; } = null!;
}
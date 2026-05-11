using System;

namespace Domain.Entities.Aircraft;

public sealed class AircraftManufacturer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Navigation
    public ICollection<AircraftModel> AircraftModels { get; set; } = [];
}
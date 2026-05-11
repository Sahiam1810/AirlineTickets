using System;
using Domain.Common;

namespace Domain.Entities.Aircraft;

public sealed class AircraftManufacturer : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Navigation
    public ICollection<AircraftModel> AircraftModels { get; set; } = [];
}
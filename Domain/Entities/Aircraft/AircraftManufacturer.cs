using System;
using Domain.Common;
using Domain.ValueObjects.Aircraft;

namespace Domain.Entities.Aircraft;

public sealed class AircraftManufacturer : BaseEntity<int>
{
    public ManufacturerName Name { get; private set; } = null!;
    public CountryName Country { get; private set; } = null!;

    private AircraftManufacturer() { }

    public AircraftManufacturer(ManufacturerName name, CountryName country)
    {
        Name = name;
        Country = country;
    }

    public void Update(ManufacturerName name, CountryName country)
    {
        Name = name;
        Country = country;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<AircraftModel> AircraftModels { get; set; } = [];
}

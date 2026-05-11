using System;
using Domain.Common;

namespace Domain.Entities.Aircraft;

public sealed class AircraftModel : BaseEntity<int>
{
    public int ManufacturerId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public decimal? MaxTakeoffWeightKg { get; set; }
    public decimal? FuelConsumptionKgH { get; set; }
    public int? CruisingSpeedKmh { get; set; }
    public int? CruisingAltitudeFt { get; set; }

    // Navigation
    public AircraftManufacturer Manufacturer { get; set; } = null!;
    public ICollection<AircraftUnit> AircraftUnits { get; set; } = [];
}
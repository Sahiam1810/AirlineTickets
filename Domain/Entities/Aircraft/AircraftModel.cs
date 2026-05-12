using System;
using Domain.Common;
using Domain.ValueObjects.Aircraft;

namespace Domain.Entities.Aircraft;

public sealed class AircraftModel : BaseEntity<int>
{
    public int ManufacturerId { get; private set; }
    public ModelName ModelName { get; private set; } = null!;
    public MaxCapacity MaxCapacity { get; private set; } = null!;
    public WeightKg? MaxTakeoffWeightKg { get; private set; }
    public FuelConsumption? FuelConsumptionKgPerHour { get; private set; }
    public SpeedKmh? CruiseSpeedKmh { get; private set; }
    public AltitudeFt? CruiseAltitudeFt { get; private set; }

    private AircraftModel() { }

    public AircraftModel(
        int manufacturerId,
        ModelName modelName,
        MaxCapacity maxCapacity,
        WeightKg? maxTakeoffWeightKg,
        FuelConsumption? fuelConsumptionKgPerHour,
        SpeedKmh? cruiseSpeedKmh,
        AltitudeFt? cruiseAltitudeFt)
    {
        ValidateManufacturer(manufacturerId);

        ManufacturerId = manufacturerId;
        ModelName = modelName;
        MaxCapacity = maxCapacity;
        MaxTakeoffWeightKg = maxTakeoffWeightKg;
        FuelConsumptionKgPerHour = fuelConsumptionKgPerHour;
        CruiseSpeedKmh = cruiseSpeedKmh;
        CruiseAltitudeFt = cruiseAltitudeFt;
    }

    public void Update(
        int manufacturerId,
        ModelName modelName,
        MaxCapacity maxCapacity,
        WeightKg? maxTakeoffWeightKg,
        FuelConsumption? fuelConsumptionKgPerHour,
        SpeedKmh? cruiseSpeedKmh,
        AltitudeFt? cruiseAltitudeFt)
    {
        ValidateManufacturer(manufacturerId);

        ManufacturerId = manufacturerId;
        ModelName = modelName;
        MaxCapacity = maxCapacity;
        MaxTakeoffWeightKg = maxTakeoffWeightKg;
        FuelConsumptionKgPerHour = fuelConsumptionKgPerHour;
        CruiseSpeedKmh = cruiseSpeedKmh;
        CruiseAltitudeFt = cruiseAltitudeFt;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateManufacturer(int manufacturerId)
    {
        if (manufacturerId <= 0)
            throw new ArgumentException("Manufacturer id is required", nameof(manufacturerId));
    }

    // Navigation
    public AircraftManufacturer Manufacturer { get; set; } = null!;
    public ICollection<AircraftUnit> AircraftUnits { get; set; } = [];
}

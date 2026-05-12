using System;
using Domain.Common;
using Domain.Entities.Airlines;
using Domain.ValueObjects.Aircraft;

namespace Domain.Entities.Aircraft;

public sealed class AircraftUnit : BaseEntity<int>
{
    public int AircraftModelId { get; private set; }
    public int AirlineId { get; private set; }
    public Registration Registration { get; private set; } = null!;
    public ManufactureDate? ManufactureDate { get; private set; }
    public bool IsActive { get; private set; }

    private AircraftUnit() { }

    public AircraftUnit(
        int aircraftModelId,
        int airlineId,
        Registration registration,
        ManufactureDate? manufactureDate,
        bool isActive)
    {
        Validate(aircraftModelId, airlineId);

        AircraftModelId = aircraftModelId;
        AirlineId = airlineId;
        Registration = registration;
        ManufactureDate = manufactureDate;
        IsActive = isActive;
    }

    public void Update(
        int aircraftModelId,
        int airlineId,
        Registration registration,
        ManufactureDate? manufactureDate,
        bool isActive)
    {
        Validate(aircraftModelId, airlineId);

        AircraftModelId = aircraftModelId;
        AirlineId = airlineId;
        Registration = registration;
        ManufactureDate = manufactureDate;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int aircraftModelId, int airlineId)
    {
        if (aircraftModelId <= 0)
            throw new ArgumentException("Aircraft model id is required", nameof(aircraftModelId));

        if (airlineId <= 0)
            throw new ArgumentException("Airline id is required", nameof(airlineId));
    }

    // Navigation
    public AircraftModel AircraftModel { get; set; } = null!;
    public Airline Airline { get; set; } = null!;
    public ICollection<CabinConfiguration> CabinConfigurations { get; set; } = [];
}

using System;
using Domain.Entities.Airlines;

namespace Domain.Entities.Aircraft;

public sealed class AircraftUnit
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public int AirlineId { get; set; }
    public string Registration { get; set; } = string.Empty;
    public DateOnly? ManufactureDate { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public AircraftModel Model { get; set; } = null!;
    public Airline Airline { get; set; } = null!;
    public ICollection<CabinConfiguration> CabinConfigurations { get; set; } = [];
}
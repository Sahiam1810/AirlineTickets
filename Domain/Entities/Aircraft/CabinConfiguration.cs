using System;

namespace Domain.Entities.Aircraft;

public sealed class CabinConfiguration
{
    public int Id { get; set; }
    public int AircraftUnitId { get; set; }
    public int CabinTypeId { get; set; }
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public int SeatsPerRow { get; set; }
    public string SeatLetters { get; set; } = string.Empty;

    // Navigation
    public AircraftUnit AircraftUnit { get; set; } = null!;
    public CabinType CabinType { get; set; } = null!;
}
namespace Api.Dtos.CabinConfigurations;

public sealed class CabinConfigurationDto
{
    public int Id { get; init; }
    public int AircraftId { get; init; }
    public string AircraftRegistration { get; init; } = default!;
    public int CabinTypeId { get; init; }
    public string CabinTypeName { get; init; } = default!;
    public int RowStart { get; init; }
    public int RowEnd { get; init; }
    public int SeatsPerRow { get; init; }
    public string SeatLetters { get; init; } = default!;
}

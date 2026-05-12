namespace Api.Dtos.CabinConfigurations;

public sealed class UpdateCabinConfigurationRequest
{
    public int AircraftId { get; init; }
    public int CabinTypeId { get; init; }
    public int RowStart { get; init; }
    public int RowEnd { get; init; }
    public int SeatsPerRow { get; init; }
    public string SeatLetters { get; init; } = default!;
}

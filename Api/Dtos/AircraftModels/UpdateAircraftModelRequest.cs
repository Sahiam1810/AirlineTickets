namespace Api.Dtos.AircraftModels;

public sealed class UpdateAircraftModelRequest
{
    public int ManufacturerId { get; init; }
    public string ModelName { get; init; } = default!;
    public int MaxCapacity { get; init; }
    public decimal? MaxTakeoffWeightKg { get; init; }
    public decimal? FuelConsumptionKgPerHour { get; init; }
    public int? CruiseSpeedKmh { get; init; }
    public int? CruiseAltitudeFt { get; init; }
}

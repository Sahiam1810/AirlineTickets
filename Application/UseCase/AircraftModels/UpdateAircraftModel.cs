using MediatR;

namespace Application.UseCase.AircraftModels;

public sealed record UpdateAircraftModel(
    int Id,
    int ManufacturerId,
    string ModelName,
    int MaxCapacity,
    decimal? MaxTakeoffWeightKg,
    decimal? FuelConsumptionKgPerHour,
    int? CruiseSpeedKmh,
    int? CruiseAltitudeFt) : IRequest;

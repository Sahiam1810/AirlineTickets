using MediatR;

namespace Application.UseCase.Aircraft;

public sealed record UpdateAircraft(
    int Id,
    int AircraftModelId,
    int AirlineId,
    string Registration,
    DateOnly? ManufactureDate,
    bool IsActive) : IRequest;

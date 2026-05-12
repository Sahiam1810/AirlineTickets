using MediatR;

namespace Application.UseCase.Aircraft;

public sealed record CreateAircraft(
    int AircraftModelId,
    int AirlineId,
    string Registration,
    DateOnly? ManufactureDate,
    bool IsActive) : IRequest<int>;

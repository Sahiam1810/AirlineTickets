using MediatR;

namespace Application.UseCase.AirportAirlines;

public sealed record CreateAirportAirline(
    int AirportId,
    int AirlineId,
    string? Terminal,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsActive) : IRequest<int>;

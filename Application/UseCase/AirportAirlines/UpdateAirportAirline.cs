using MediatR;

namespace Application.UseCase.AirportAirlines;

public sealed record UpdateAirportAirline(
    int Id,
    string? Terminal,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsActive) : IRequest;

using MediatR;

namespace Application.UseCase.RouteStops;

public sealed record UpdateRouteStop(
    int Id,
    int StopAirportId,
    int Order,
    int StopDurationMinutes) : IRequest;

using MediatR;

namespace Application.UseCase.RouteStops;

public sealed record CreateRouteStop(
    int RouteId,
    int StopAirportId,
    int Order,
    int StopDurationMinutes) : IRequest<int>;

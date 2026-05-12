using MediatR;

namespace Application.UseCase.RouteStops;

public sealed record DeleteRouteStop(int Id) : IRequest;

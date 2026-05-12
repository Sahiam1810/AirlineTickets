using MediatR;

namespace Application.UseCase.Routes;

public sealed record UpdateRoute(
    int Id,
    int OriginAirportId,
    int DestinationAirportId,
    int? DistanceKm,
    int? EstimatedDurationMinutes) : IRequest;

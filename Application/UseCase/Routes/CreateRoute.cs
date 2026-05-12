using MediatR;

namespace Application.UseCase.Routes;

public sealed record CreateRoute(
    int OriginAirportId,
    int DestinationAirportId,
    int? DistanceKm,
    int? EstimatedDurationMinutes) : IRequest<int>;

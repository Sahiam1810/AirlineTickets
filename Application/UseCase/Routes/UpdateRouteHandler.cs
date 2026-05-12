using Application.Abstractions;
using Domain.ValueObjects.Routes;
using MediatR;

namespace Application.UseCase.Routes;

public sealed class UpdateRouteHandler(IUnitOfWork uow) : IRequestHandler<UpdateRoute>
{
    public async Task Handle(UpdateRoute request, CancellationToken ct)
    {
        var route = await uow.Routes.GetByIdAsync(request.Id, ct);
        if (route is null)
            throw new Exception($"Route with id {request.Id} not found.");

        if (request.OriginAirportId == request.DestinationAirportId)
            throw new Exception("Origin airport and destination airport cannot be the same.");

        var originAirport = await uow.Airports.GetByIdAsync(request.OriginAirportId, ct);
        if (originAirport is null)
            throw new Exception($"Origin airport with id {request.OriginAirportId} not found.");

        var destinationAirport = await uow.Airports.GetByIdAsync(request.DestinationAirportId, ct);
        if (destinationAirport is null)
            throw new Exception($"Destination airport with id {request.DestinationAirportId} not found.");

        if (await uow.Routes.ExistsAsync(request.OriginAirportId, request.DestinationAirportId, request.Id, ct))
            throw new Exception("Route already exists for this origin and destination.");

        route.Update(
            request.OriginAirportId,
            request.DestinationAirportId,
            DistanceKm.Create(request.DistanceKm),
            DurationMinutes.Create(request.EstimatedDurationMinutes));

        await uow.Routes.UpdateAsync(route, ct);
        await uow.SaveChangesAsync(ct);
    }
}

using Application.Abstractions;
using Domain.ValueObjects.Routes;
using MediatR;

namespace Application.UseCase.RouteStops;

public sealed class UpdateRouteStopHandler(IUnitOfWork uow) : IRequestHandler<UpdateRouteStop>
{
    public async Task Handle(UpdateRouteStop request, CancellationToken ct)
    {
        var routeStop = await uow.RouteStops.GetByIdAsync(request.Id, ct);
        if (routeStop is null)
            throw new Exception($"RouteStop with id {request.Id} not found.");

        var route = await uow.Routes.GetByIdAsync(routeStop.RouteId, ct);
        if (route is null)
            throw new Exception($"Route with id {routeStop.RouteId} not found.");

        var airport = await uow.Airports.GetByIdAsync(request.StopAirportId, ct);
        if (airport is null)
            throw new Exception($"Airport with id {request.StopAirportId} not found.");

        if (request.StopAirportId == route.OriginAirportId || request.StopAirportId == route.DestinationAirportId)
            throw new Exception("Stop airport cannot be the same as route origin or destination.");

        var order = StopOrder.Create(request.Order);
        var duration = StopDurationMinutes.Create(request.StopDurationMinutes);

        if (await uow.RouteStops.ExistsAsync(routeStop.RouteId, order.Value, request.Id, ct))
            throw new Exception("RouteStop already exists for this route and order.");

        routeStop.Update(request.StopAirportId, order, duration);

        await uow.RouteStops.UpdateAsync(routeStop, ct);
        await uow.SaveChangesAsync(ct);
    }
}

using Application.Abstractions;
using Domain.Entities.Routes;
using Domain.ValueObjects.Routes;
using MediatR;

namespace Application.UseCase.RouteStops;

public sealed class CreateRouteStopHandler : IRequestHandler<CreateRouteStop, int>
{
    private readonly IUnitOfWork uow;

    public CreateRouteStopHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateRouteStop req, CancellationToken ct)
    {
        var route = await uow.Routes.GetByIdAsync(req.RouteId, ct);
        if (route is null)
            throw new Exception($"Route with id {req.RouteId} not found.");

        var airport = await uow.Airports.GetByIdAsync(req.StopAirportId, ct);
        if (airport is null)
            throw new Exception($"Airport with id {req.StopAirportId} not found.");

        if (req.StopAirportId == route.OriginAirportId || req.StopAirportId == route.DestinationAirportId)
            throw new Exception("Stop airport cannot be the same as route origin or destination.");

        var order = StopOrder.Create(req.Order);
        var duration = StopDurationMinutes.Create(req.StopDurationMinutes);

        if (await uow.RouteStops.ExistsAsync(req.RouteId, order.Value, null, ct))
            throw new Exception("RouteStop already exists for this route and order.");

        var routeStop = new RouteStop(req.RouteId, req.StopAirportId, order, duration);

        await uow.RouteStops.AddAsync(routeStop, ct);
        await uow.SaveChangesAsync(ct);
        return routeStop.Id;
    }
}

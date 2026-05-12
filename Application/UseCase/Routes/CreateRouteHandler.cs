using Application.Abstractions;
using Domain.ValueObjects.Routes;
using MediatR;

namespace Application.UseCase.Routes;

public sealed class CreateRouteHandler : IRequestHandler<CreateRoute, int>
{
    private readonly IUnitOfWork uow;

    public CreateRouteHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateRoute req, CancellationToken ct)
    {
        if (req.OriginAirportId == req.DestinationAirportId)
            throw new Exception("Origin airport and destination airport cannot be the same.");

        var originAirport = await uow.Airports.GetByIdAsync(req.OriginAirportId, ct);
        if (originAirport is null)
            throw new Exception($"Origin airport with id {req.OriginAirportId} not found.");

        var destinationAirport = await uow.Airports.GetByIdAsync(req.DestinationAirportId, ct);
        if (destinationAirport is null)
            throw new Exception($"Destination airport with id {req.DestinationAirportId} not found.");

        if (await uow.Routes.ExistsAsync(req.OriginAirportId, req.DestinationAirportId, null, ct))
            throw new Exception("Route already exists for this origin and destination.");

        var route = new Domain.Entities.Routes.Route(
            req.OriginAirportId,
            req.DestinationAirportId,
            DistanceKm.Create(req.DistanceKm),
            DurationMinutes.Create(req.EstimatedDurationMinutes));

        await uow.Routes.AddAsync(route, ct);
        await uow.SaveChangesAsync(ct);
        return route.Id;
    }
}

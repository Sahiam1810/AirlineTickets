using Application.Abstractions;
using MediatR;

namespace Application.UseCase.RouteStops;

public sealed class DeleteRouteStopHandler(IUnitOfWork uow) : IRequestHandler<DeleteRouteStop>
{
    public async Task Handle(DeleteRouteStop request, CancellationToken ct)
    {
        var routeStop = await uow.RouteStops.GetByIdAsync(request.Id, ct);

        if (routeStop is null)
            throw new Exception($"RouteStop with id {request.Id} not found.");

        await uow.RouteStops.RemoveAsync(routeStop, ct);
        await uow.SaveChangesAsync(ct);
    }
}

using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Routes;

public sealed class DeleteRouteHandler(IUnitOfWork uow) : IRequestHandler<DeleteRoute>
{
    public async Task Handle(DeleteRoute request, CancellationToken ct)
    {
        var route = await uow.Routes.GetByIdAsync(request.Id, ct);

        if (route is null)
            throw new Exception($"Route with id {request.Id} not found.");

        await uow.Routes.RemoveAsync(route, ct);
        await uow.SaveChangesAsync(ct);
    }
}

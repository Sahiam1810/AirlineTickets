using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Aircraft;

public sealed class DeleteAircraftHandler(IUnitOfWork uow) : IRequestHandler<DeleteAircraft>
{
    public async Task Handle(DeleteAircraft request, CancellationToken ct)
    {
        var aircraft = await uow.Aircraft.GetByIdAsync(request.Id, ct);

        if (aircraft is null)
            throw new Exception($"Aircraft with id {request.Id} not found.");

        await uow.Aircraft.RemoveAsync(aircraft, ct);
        await uow.SaveChangesAsync(ct);
    }
}

using Application.Abstractions;
using MediatR;

namespace Application.UseCase.AircraftModels;

public sealed class DeleteAircraftModelHandler(IUnitOfWork uow) : IRequestHandler<DeleteAircraftModel>
{
    public async Task Handle(DeleteAircraftModel request, CancellationToken ct)
    {
        var aircraftModel = await uow.AircraftModels.GetByIdAsync(request.Id, ct);

        if (aircraftModel is null)
            throw new Exception($"AircraftModel with id {request.Id} not found.");

        await uow.AircraftModels.RemoveAsync(aircraftModel, ct);
        await uow.SaveChangesAsync(ct);
    }
}

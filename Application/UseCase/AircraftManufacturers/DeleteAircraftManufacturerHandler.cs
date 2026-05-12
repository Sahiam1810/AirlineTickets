using Application.Abstractions;
using MediatR;

namespace Application.UseCase.AircraftManufacturers;

public sealed class DeleteAircraftManufacturerHandler(IUnitOfWork uow) : IRequestHandler<DeleteAircraftManufacturer>
{
    public async Task Handle(DeleteAircraftManufacturer request, CancellationToken ct)
    {
        var aircraftManufacturer = await uow.AircraftManufacturers.GetByIdAsync(request.Id, ct);

        if (aircraftManufacturer is null)
            throw new Exception($"AircraftManufacturer with id {request.Id} not found.");

        await uow.AircraftManufacturers.RemoveAsync(aircraftManufacturer, ct);
        await uow.SaveChangesAsync(ct);
    }
}

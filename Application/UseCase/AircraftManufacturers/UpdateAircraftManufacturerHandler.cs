using Application.Abstractions;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.AircraftManufacturers;

public sealed class UpdateAircraftManufacturerHandler(IUnitOfWork uow) : IRequestHandler<UpdateAircraftManufacturer>
{
    public async Task Handle(UpdateAircraftManufacturer request, CancellationToken ct)
    {
        var aircraftManufacturer = await uow.AircraftManufacturers.GetByIdAsync(request.Id, ct);

        if (aircraftManufacturer is null)
            throw new Exception($"AircraftManufacturer with id {request.Id} not found.");

        var name = ManufacturerName.Create(request.Name);
        var country = CountryName.Create(request.Country);
        var sameName = string.Equals(aircraftManufacturer.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.AircraftManufacturers.ExistsByNameAsync(name, ct))
            throw new Exception($"AircraftManufacturer with name {name.Value} already exists.");

        aircraftManufacturer.Update(name, country);

        await uow.AircraftManufacturers.UpdateAsync(aircraftManufacturer, ct);
        await uow.SaveChangesAsync(ct);
    }
}

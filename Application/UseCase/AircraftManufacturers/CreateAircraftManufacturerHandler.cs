using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.AircraftManufacturers;

public sealed class CreateAircraftManufacturerHandler : IRequestHandler<CreateAircraftManufacturer, int>
{
    private readonly IUnitOfWork uow;

    public CreateAircraftManufacturerHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAircraftManufacturer req, CancellationToken ct)
    {
        var name = ManufacturerName.Create(req.Name);
        var country = CountryName.Create(req.Country);

        if (await uow.AircraftManufacturers.ExistsByNameAsync(name, ct))
            throw new Exception($"AircraftManufacturer with name {name.Value} already exists.");

        var aircraftManufacturer = new AircraftManufacturer(name, country);

        await uow.AircraftManufacturers.AddAsync(aircraftManufacturer, ct);
        await uow.SaveChangesAsync(ct);
        return aircraftManufacturer.Id;
    }
}

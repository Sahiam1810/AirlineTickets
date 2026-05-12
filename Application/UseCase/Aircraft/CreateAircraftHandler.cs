using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.Aircraft;

public sealed class CreateAircraftHandler : IRequestHandler<CreateAircraft, int>
{
    private readonly IUnitOfWork uow;

    public CreateAircraftHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAircraft req, CancellationToken ct)
    {
        var aircraftModel = await uow.AircraftModels.GetByIdAsync(req.AircraftModelId, ct);
        if (aircraftModel is null)
            throw new Exception($"AircraftModel with id {req.AircraftModelId} not found.");

        var airline = await uow.Airlines.GetByIdAsync(req.AirlineId, ct);
        if (airline is null)
            throw new Exception($"Airline with id {req.AirlineId} not found.");

        var registration = Registration.Create(req.Registration);
        var manufactureDate = ManufactureDate.Create(req.ManufactureDate);

        if (await uow.Aircraft.ExistsByRegistrationAsync(registration, ct))
            throw new Exception($"Aircraft with registration {registration.Value} already exists.");

        var aircraft = new AircraftUnit(
            req.AircraftModelId,
            req.AirlineId,
            registration,
            manufactureDate,
            req.IsActive);

        await uow.Aircraft.AddAsync(aircraft, ct);
        await uow.SaveChangesAsync(ct);
        return aircraft.Id;
    }
}

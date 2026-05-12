using Application.Abstractions;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.Aircraft;

public sealed class UpdateAircraftHandler(IUnitOfWork uow) : IRequestHandler<UpdateAircraft>
{
    public async Task Handle(UpdateAircraft request, CancellationToken ct)
    {
        var aircraft = await uow.Aircraft.GetByIdAsync(request.Id, ct);
        if (aircraft is null)
            throw new Exception($"Aircraft with id {request.Id} not found.");

        var aircraftModel = await uow.AircraftModels.GetByIdAsync(request.AircraftModelId, ct);
        if (aircraftModel is null)
            throw new Exception($"AircraftModel with id {request.AircraftModelId} not found.");

        var airline = await uow.Airlines.GetByIdAsync(request.AirlineId, ct);
        if (airline is null)
            throw new Exception($"Airline with id {request.AirlineId} not found.");

        var registration = Registration.Create(request.Registration);
        var sameRegistration = string.Equals(aircraft.Registration.Value, registration.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameRegistration && await uow.Aircraft.ExistsByRegistrationAsync(registration, ct))
            throw new Exception($"Aircraft with registration {registration.Value} already exists.");

        aircraft.Update(
            request.AircraftModelId,
            request.AirlineId,
            registration,
            ManufactureDate.Create(request.ManufactureDate),
            request.IsActive);

        await uow.Aircraft.UpdateAsync(aircraft, ct);
        await uow.SaveChangesAsync(ct);
    }
}

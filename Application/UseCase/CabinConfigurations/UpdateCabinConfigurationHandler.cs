using Application.Abstractions;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.CabinConfigurations;

public sealed class UpdateCabinConfigurationHandler(IUnitOfWork uow) : IRequestHandler<UpdateCabinConfiguration>
{
    public async Task Handle(UpdateCabinConfiguration request, CancellationToken ct)
    {
        var cabinConfiguration = await uow.CabinConfigurations.GetByIdAsync(request.Id, ct);
        if (cabinConfiguration is null)
            throw new Exception($"CabinConfiguration with id {request.Id} not found.");

        var aircraft = await uow.Aircraft.GetByIdAsync(request.AircraftId, ct);
        if (aircraft is null)
            throw new Exception($"Aircraft with id {request.AircraftId} not found.");

        var cabinType = await uow.CabinTypes.GetByIdAsync(request.CabinTypeId, ct);
        if (cabinType is null)
            throw new Exception($"CabinType with id {request.CabinTypeId} not found.");

        if (await uow.CabinConfigurations.ExistsAsync(request.AircraftId, request.CabinTypeId, request.Id, ct))
            throw new Exception("CabinConfiguration already exists for this aircraft and cabin type.");

        cabinConfiguration.Update(
            request.AircraftId,
            request.CabinTypeId,
            RowNumber.Create(request.RowStart),
            RowNumber.Create(request.RowEnd),
            SeatsPerRow.Create(request.SeatsPerRow),
            SeatLetters.Create(request.SeatLetters));

        await uow.CabinConfigurations.UpdateAsync(cabinConfiguration, ct);
        await uow.SaveChangesAsync(ct);
    }
}

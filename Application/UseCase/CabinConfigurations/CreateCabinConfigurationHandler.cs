using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.CabinConfigurations;

public sealed class CreateCabinConfigurationHandler : IRequestHandler<CreateCabinConfiguration, int>
{
    private readonly IUnitOfWork uow;

    public CreateCabinConfigurationHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateCabinConfiguration req, CancellationToken ct)
    {
        var aircraft = await uow.Aircraft.GetByIdAsync(req.AircraftId, ct);
        if (aircraft is null)
            throw new Exception($"Aircraft with id {req.AircraftId} not found.");

        var cabinType = await uow.CabinTypes.GetByIdAsync(req.CabinTypeId, ct);
        if (cabinType is null)
            throw new Exception($"CabinType with id {req.CabinTypeId} not found.");

        if (await uow.CabinConfigurations.ExistsAsync(req.AircraftId, req.CabinTypeId, null, ct))
            throw new Exception("CabinConfiguration already exists for this aircraft and cabin type.");

        var cabinConfiguration = new CabinConfiguration(
            req.AircraftId,
            req.CabinTypeId,
            RowNumber.Create(req.RowStart),
            RowNumber.Create(req.RowEnd),
            SeatsPerRow.Create(req.SeatsPerRow),
            SeatLetters.Create(req.SeatLetters));

        await uow.CabinConfigurations.AddAsync(cabinConfiguration, ct);
        await uow.SaveChangesAsync(ct);
        return cabinConfiguration.Id;
    }
}

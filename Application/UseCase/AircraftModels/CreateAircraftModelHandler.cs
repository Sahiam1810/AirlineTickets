using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.AircraftModels;

public sealed class CreateAircraftModelHandler : IRequestHandler<CreateAircraftModel, int>
{
    private readonly IUnitOfWork uow;

    public CreateAircraftModelHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAircraftModel req, CancellationToken ct)
    {
        var manufacturer = await uow.AircraftManufacturers.GetByIdAsync(req.ManufacturerId, ct);
        if (manufacturer is null)
            throw new Exception($"AircraftManufacturer with id {req.ManufacturerId} not found.");

        var modelName = ModelName.Create(req.ModelName);
        var maxCapacity = MaxCapacity.Create(req.MaxCapacity);
        var maxTakeoffWeightKg = WeightKg.Create(req.MaxTakeoffWeightKg);
        var fuelConsumptionKgPerHour = FuelConsumption.Create(req.FuelConsumptionKgPerHour);
        var cruiseSpeedKmh = SpeedKmh.Create(req.CruiseSpeedKmh);
        var cruiseAltitudeFt = AltitudeFt.Create(req.CruiseAltitudeFt);

        if (await uow.AircraftModels.ExistsByNameAsync(req.ManufacturerId, modelName, ct))
            throw new Exception($"AircraftModel with name {modelName.Value} already exists for this manufacturer.");

        var aircraftModel = new AircraftModel(
            req.ManufacturerId,
            modelName,
            maxCapacity,
            maxTakeoffWeightKg,
            fuelConsumptionKgPerHour,
            cruiseSpeedKmh,
            cruiseAltitudeFt);

        await uow.AircraftModels.AddAsync(aircraftModel, ct);
        await uow.SaveChangesAsync(ct);
        return aircraftModel.Id;
    }
}

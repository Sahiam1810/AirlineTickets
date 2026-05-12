using Application.Abstractions;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.AircraftModels;

public sealed class UpdateAircraftModelHandler(IUnitOfWork uow) : IRequestHandler<UpdateAircraftModel>
{
    public async Task Handle(UpdateAircraftModel request, CancellationToken ct)
    {
        var aircraftModel = await uow.AircraftModels.GetByIdAsync(request.Id, ct);
        if (aircraftModel is null)
            throw new Exception($"AircraftModel with id {request.Id} not found.");

        var manufacturer = await uow.AircraftManufacturers.GetByIdAsync(request.ManufacturerId, ct);
        if (manufacturer is null)
            throw new Exception($"AircraftManufacturer with id {request.ManufacturerId} not found.");

        var modelName = ModelName.Create(request.ModelName);
        var sameModel = aircraftModel.ManufacturerId == request.ManufacturerId &&
            string.Equals(aircraftModel.ModelName.Value, modelName.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameModel && await uow.AircraftModels.ExistsByNameAsync(request.ManufacturerId, modelName, ct))
            throw new Exception($"AircraftModel with name {modelName.Value} already exists for this manufacturer.");

        aircraftModel.Update(
            request.ManufacturerId,
            modelName,
            MaxCapacity.Create(request.MaxCapacity),
            WeightKg.Create(request.MaxTakeoffWeightKg),
            FuelConsumption.Create(request.FuelConsumptionKgPerHour),
            SpeedKmh.Create(request.CruiseSpeedKmh),
            AltitudeFt.Create(request.CruiseAltitudeFt));

        await uow.AircraftModels.UpdateAsync(aircraftModel, ct);
        await uow.SaveChangesAsync(ct);
    }
}

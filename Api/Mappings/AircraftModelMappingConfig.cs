using Api.Dtos.AircraftModels;
using Application.UseCase.AircraftModels;
using Domain.Entities.Aircraft;
using Mapster;

namespace Api.Mappings;

public sealed class AircraftModelMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AircraftModel, AircraftModelDto>()
            .Map(dest => dest.ManufacturerName, src => src.Manufacturer.Name.Value)
            .Map(dest => dest.ModelName, src => src.ModelName.Value)
            .Map(dest => dest.MaxCapacity, src => src.MaxCapacity.Value)
            .Map(dest => dest.MaxTakeoffWeightKg, src => src.MaxTakeoffWeightKg == null ? (decimal?)null : src.MaxTakeoffWeightKg.Value)
            .Map(dest => dest.FuelConsumptionKgPerHour, src => src.FuelConsumptionKgPerHour == null ? (decimal?)null : src.FuelConsumptionKgPerHour.Value)
            .Map(dest => dest.CruiseSpeedKmh, src => src.CruiseSpeedKmh == null ? (int?)null : src.CruiseSpeedKmh.Value)
            .Map(dest => dest.CruiseAltitudeFt, src => src.CruiseAltitudeFt == null ? (int?)null : src.CruiseAltitudeFt.Value);

        config.NewConfig<CreateAircraftModelRequest, CreateAircraftModel>();
        config.NewConfig<UpdateAircraftModelRequest, UpdateAircraftModel>()
            .Map(dest => dest.Id, _ => 0);
    }
}

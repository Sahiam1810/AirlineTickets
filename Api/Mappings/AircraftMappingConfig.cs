using Api.Dtos.Aircraft;
using Application.UseCase.Aircraft;
using Domain.Entities.Aircraft;
using Mapster;

namespace Api.Mappings;

public sealed class AircraftMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AircraftUnit, AircraftDto>()
            .Map(dest => dest.AircraftModelName, src => src.AircraftModel.ModelName.Value)
            .Map(dest => dest.AirlineName, src => src.Airline.Name.Value)
            .Map(dest => dest.Registration, src => src.Registration.Value)
            .Map(dest => dest.ManufactureDate, src => src.ManufactureDate == null ? (DateOnly?)null : src.ManufactureDate.Value);

        config.NewConfig<CreateAircraftRequest, CreateAircraft>();
        config.NewConfig<UpdateAircraftRequest, UpdateAircraft>()
            .Map(dest => dest.Id, _ => 0);
    }
}

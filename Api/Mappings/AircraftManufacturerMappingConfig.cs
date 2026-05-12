using Api.Dtos.AircraftManufacturers;
using Application.UseCase.AircraftManufacturers;
using Domain.Entities.Aircraft;
using Mapster;

namespace Api.Mappings;

public sealed class AircraftManufacturerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AircraftManufacturer, AircraftManufacturerDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Country, src => src.Country.Value);

        config.NewConfig<CreateAircraftManufacturerRequest, CreateAircraftManufacturer>();
        config.NewConfig<UpdateAircraftManufacturerRequest, UpdateAircraftManufacturer>()
            .Map(dest => dest.Id, _ => 0);
    }
}

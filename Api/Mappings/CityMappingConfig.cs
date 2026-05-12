using Api.Dtos.Cities;
using Application.UseCase.Cities;
using Domain.Entities.Geography;
using Mapster;

namespace Api.Mappings;

public sealed class CityMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<City, CityDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.RegionId, src => src.RegionId);

        config.NewConfig<CreateCityRequest, CreateCity>();
        config.NewConfig<UpdateCityRequest, UpdateCity>()
            .Map(dest => dest.Id, _ => 0);
    }
}

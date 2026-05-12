using Api.Dtos.Regions;
using Application.UseCase.Regions;
using Domain.Entities.Geography;
using Mapster;

namespace Api.Mappings;

public sealed class RegionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Region, RegionDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Type, src => src.Type.Value)
            .Map(dest => dest.CountryId, src => src.CountryId);

        config.NewConfig<CreateRegionRequest, CreateRegion>();
        config.NewConfig<UpdateRegionRequest, UpdateRegion>()
            .Map(dest => dest.Id, _ => 0);
    }
}

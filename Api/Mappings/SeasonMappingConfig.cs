using Api.Dtos.Seasons;
using Application.UseCase.Seasons;
using Domain.Entities.Routes;
using Mapster;

namespace Api.Mappings;

public sealed class SeasonMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Season, SeasonDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description == null ? null : src.Description.Value)
            .Map(dest => dest.PriceFactor, src => src.PriceFactor.Value);

        config.NewConfig<CreateSeasonRequest, CreateSeason>();
        config.NewConfig<UpdateSeasonRequest, UpdateSeason>()
            .Map(dest => dest.Id, _ => 0);
    }
}

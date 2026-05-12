using Api.Dtos.Countries;
using Application.UseCase.Countries;
using Domain.Entities.Geography;
using Mapster;

namespace Api.Mappings;

public sealed class CountryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Country, CountryDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.ContinentId, src => src.ContinentId);

        config.NewConfig<CreateCountryRequest, CreateCountry>();
        config.NewConfig<UpdateCountryRequest, UpdateCountry>()
            .Map(dest => dest.Id, _ => 0);
    }
}

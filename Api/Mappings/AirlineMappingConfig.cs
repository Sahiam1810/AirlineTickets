using Api.Dtos.Airlines;
using Application.UseCase.Airlines;
using Domain.Entities.Airlines;
using Mapster;

namespace Api.Mappings;

public sealed class AirlineMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Airline, AirlineDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.IataCode, src => src.IataCode.Value)
            .Map(dest => dest.CountryName, src => src.Country.Name.Value);

        config.NewConfig<CreateAirlineRequest, CreateAirline>();
        config.NewConfig<UpdateAirlineRequest, UpdateAirline>()
            .Map(dest => dest.Id, _ => 0);
    }
}

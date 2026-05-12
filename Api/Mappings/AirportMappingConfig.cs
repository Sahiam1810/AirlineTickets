using Api.Dtos.Airports;
using Application.UseCase.Airports;
using Domain.Entities.Airlines;
using Mapster;

namespace Api.Mappings;

public sealed class AirportMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Airport, AirportDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.IataCode, src => src.IataCode.Value)
            .Map(dest => dest.IcaoCode, src => src.IcaoCode == null ? null : src.IcaoCode.Value)
            .Map(dest => dest.CityName, src => src.City.Name.Value);

        config.NewConfig<CreateAirportRequest, CreateAirport>();
        config.NewConfig<UpdateAirportRequest, UpdateAirport>()
            .Map(dest => dest.Id, _ => 0);
    }
}

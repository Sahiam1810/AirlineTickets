using Api.Dtos.AirportAirlines;
using Application.UseCase.AirportAirlines;
using Domain.Entities.Airlines;
using Mapster;

namespace Api.Mappings;

public sealed class AirportAirlineMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AirportAirline, AirportAirlineDto>()
            .Map(dest => dest.AirportName, src => src.Airport.Name.Value)
            .Map(dest => dest.AirportIataCode, src => src.Airport.IataCode.Value)
            .Map(dest => dest.AirlineName, src => src.Airline.Name.Value)
            .Map(dest => dest.AirlineIataCode, src => src.Airline.IataCode.Value)
            .Map(dest => dest.Terminal, src => src.Terminal == null ? null : src.Terminal.Value);

        config.NewConfig<CreateAirportAirlineRequest, CreateAirportAirline>();
        config.NewConfig<UpdateAirportAirlineRequest, UpdateAirportAirline>()
            .Map(dest => dest.Id, _ => 0);
    }
}

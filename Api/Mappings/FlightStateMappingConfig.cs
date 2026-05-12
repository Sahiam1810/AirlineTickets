using Api.Dtos.FlightStates;
using Application.UseCase.FlightStates;
using Domain.Entities.Flights;
using Mapster;

namespace Api.Mappings;

public sealed class FlightStateMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlightState, FlightStateDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateFlightStateRequest, CreateFlightState>();
        config.NewConfig<UpdateFlightStateRequest, UpdateFlightState>()
            .Map(dest => dest.Id, _ => 0);
    }
}

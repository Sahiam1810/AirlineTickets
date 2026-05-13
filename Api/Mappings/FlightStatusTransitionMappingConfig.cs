using Api.Dtos.FlightStatusTransitions;
using Application.UseCase.FlightStatusTransitions;
using Domain.Entities.Flights;
using Mapster;

namespace Api.Mappings;

public sealed class FlightStatusTransitionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlightStatusTransition, FlightStatusTransitionDto>()
            .Map(dest => dest.FromStateName, src => src.FromState.Name.Value)
            .Map(dest => dest.ToStateName, src => src.ToState.Name.Value);

        config.NewConfig<CreateFlightStatusTransitionRequest, CreateFlightStatusTransition>();
        config.NewConfig<UpdateFlightStatusTransitionRequest, UpdateFlightStatusTransition>()
            .Map(dest => dest.Id, _ => 0);
    }
}

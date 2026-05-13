using Api.Dtos.Flights;
using Application.UseCase.Flights;
using Domain.Entities.Flights;
using Mapster;

namespace Api.Mappings;

public sealed class FlightMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Flight, FlightDto>()
            .Map(dest => dest.FlightCode, src => src.FlightCode.Value)
            .Map(dest => dest.AirlineName, src => src.Airline.Name.Value)
            .Map(dest => dest.AirlineIataCode, src => src.Airline.IataCode.Value)
            .Map(dest => dest.OriginAirportId, src => src.Route.OriginAirportId)
            .Map(dest => dest.OriginAirportName, src => src.Route.OriginAirport.Name.Value)
            .Map(dest => dest.OriginAirportIataCode, src => src.Route.OriginAirport.IataCode.Value)
            .Map(dest => dest.DestinationAirportId, src => src.Route.DestinationAirportId)
            .Map(dest => dest.DestinationAirportName, src => src.Route.DestinationAirport.Name.Value)
            .Map(dest => dest.DestinationAirportIataCode, src => src.Route.DestinationAirport.IataCode.Value)
            .Map(dest => dest.AircraftRegistration, src => src.Aircraft.Registration.Value)
            .Map(dest => dest.AircraftModelName, src => src.Aircraft.AircraftModel.ModelName.Value)
            .Map(dest => dest.TotalCapacity, src => src.TotalCapacity.Value)
            .Map(dest => dest.AvailableSeats, src => src.AvailableSeats.Value)
            .Map(dest => dest.FlightStateName, src => src.FlightState.Name.Value);

        config.NewConfig<CreateFlightRequest, CreateFlight>();
        config.NewConfig<UpdateFlightRequest, UpdateFlight>()
            .Map(dest => dest.Id, _ => 0);
        config.NewConfig<ChangeFlightStateRequest, ChangeFlightState>()
            .Map(dest => dest.Id, _ => 0);
        config.NewConfig<RescheduleFlightRequest, RescheduleFlight>()
            .Map(dest => dest.Id, _ => 0);
    }
}

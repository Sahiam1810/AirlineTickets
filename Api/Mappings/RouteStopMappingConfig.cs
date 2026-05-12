using Api.Dtos.RouteStops;
using Application.UseCase.RouteStops;
using Domain.Entities.Routes;
using Mapster;

namespace Api.Mappings;

public sealed class RouteStopMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RouteStop, RouteStopDto>()
            .Map(dest => dest.OriginAirportId, src => src.Route.OriginAirportId)
            .Map(dest => dest.OriginAirportName, src => src.Route.OriginAirport.Name.Value)
            .Map(dest => dest.OriginAirportIataCode, src => src.Route.OriginAirport.IataCode.Value)
            .Map(dest => dest.DestinationAirportId, src => src.Route.DestinationAirportId)
            .Map(dest => dest.DestinationAirportName, src => src.Route.DestinationAirport.Name.Value)
            .Map(dest => dest.DestinationAirportIataCode, src => src.Route.DestinationAirport.IataCode.Value)
            .Map(dest => dest.StopAirportName, src => src.StopAirport.Name.Value)
            .Map(dest => dest.StopAirportIataCode, src => src.StopAirport.IataCode.Value)
            .Map(dest => dest.Order, src => src.Order.Value)
            .Map(dest => dest.StopDurationMinutes, src => src.StopDurationMinutes.Value);

        config.NewConfig<CreateRouteStopRequest, CreateRouteStop>();
        config.NewConfig<UpdateRouteStopRequest, UpdateRouteStop>()
            .Map(dest => dest.Id, _ => 0);
    }
}

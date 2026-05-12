using Api.Dtos.Routes;
using Application.UseCase.Routes;
using Mapster;

namespace Api.Mappings;

public sealed class RouteMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Entities.Routes.Route, RouteDto>()
            .Map(dest => dest.OriginAirportName, src => src.OriginAirport.Name.Value)
            .Map(dest => dest.OriginAirportIataCode, src => src.OriginAirport.IataCode.Value)
            .Map(dest => dest.DestinationAirportName, src => src.DestinationAirport.Name.Value)
            .Map(dest => dest.DestinationAirportIataCode, src => src.DestinationAirport.IataCode.Value)
            .Map(dest => dest.DistanceKm, src => src.DistanceKm == null ? (int?)null : src.DistanceKm.Value)
            .Map(dest => dest.EstimatedDurationMinutes, src => src.EstimatedDurationMinutes == null ? (int?)null : src.EstimatedDurationMinutes.Value);

        config.NewConfig<CreateRouteRequest, CreateRoute>();
        config.NewConfig<UpdateRouteRequest, UpdateRoute>()
            .Map(dest => dest.Id, _ => 0);
    }
}

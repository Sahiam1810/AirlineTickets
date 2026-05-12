using Api.Dtos.Fares;
using Application.UseCase.Fares;
using Domain.Entities.Routes;
using Mapster;

namespace Api.Mappings;

public sealed class FareMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Fare, FareDto>()
            .Map(dest => dest.OriginAirportId, src => src.Route.OriginAirportId)
            .Map(dest => dest.OriginAirportName, src => src.Route.OriginAirport.Name.Value)
            .Map(dest => dest.OriginAirportIataCode, src => src.Route.OriginAirport.IataCode.Value)
            .Map(dest => dest.DestinationAirportId, src => src.Route.DestinationAirportId)
            .Map(dest => dest.DestinationAirportName, src => src.Route.DestinationAirport.Name.Value)
            .Map(dest => dest.DestinationAirportIataCode, src => src.Route.DestinationAirport.IataCode.Value)
            .Map(dest => dest.CabinTypeName, src => src.CabinType.Name.Value)
            .Map(dest => dest.PassengerTypeName, src => src.PassengerType.Name.Value)
            .Map(dest => dest.SeasonName, src => src.Season.Name.Value)
            .Map(dest => dest.BasePrice, src => src.BasePrice.Value);

        config.NewConfig<CreateFareRequest, CreateFare>();
        config.NewConfig<UpdateFareRequest, UpdateFare>()
            .Map(dest => dest.Id, _ => 0);
    }
}

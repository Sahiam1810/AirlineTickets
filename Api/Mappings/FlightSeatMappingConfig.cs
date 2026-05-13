using Api.Dtos.FlightSeats;
using Application.UseCase.FlightSeats;
using Domain.Entities.Flights;
using Mapster;

namespace Api.Mappings;

public sealed class FlightSeatMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlightSeat, FlightSeatDto>()
            .Map(dest => dest.FlightCode, src => src.Flight.FlightCode.Value)
            .Map(dest => dest.SeatCode, src => src.SeatCode.Value)
            .Map(dest => dest.CabinTypeName, src => src.CabinType.Name.Value)
            .Map(dest => dest.SeatLocationTypeName, src => src.SeatLocationType.Name.Value);

        config.NewConfig<CreateFlightSeatRequest, CreateFlightSeat>();
        config.NewConfig<UpdateFlightSeatRequest, UpdateFlightSeat>()
            .Map(dest => dest.Id, _ => 0);
    }
}

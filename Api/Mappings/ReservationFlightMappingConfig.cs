using Api.Dtos.ReservationFlights;
using Application.UseCase.ReservationFlights;
using Domain.Entities.Reservations;
using Mapster;

namespace Api.Mappings;

public sealed class ReservationFlightMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReservationFlight, ReservationFlightDto>()
            .Map(dest => dest.ReservationCode, src => src.Reservation.ReservationCode.Value)
            .Map(dest => dest.FlightCode, src => src.Flight.FlightCode.Value);

        config.NewConfig<CreateReservationFlightRequest, CreateReservationFlight>();
        config.NewConfig<UpdateReservationFlightRequest, UpdateReservationFlight>()
            .Map(dest => dest.Id, _ => 0);
    }
}

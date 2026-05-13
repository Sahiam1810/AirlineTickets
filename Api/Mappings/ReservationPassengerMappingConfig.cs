using Api.Dtos.ReservationPassengers;
using Application.UseCase.ReservationPassengers;
using Domain.Entities.Reservations;
using Mapster;

namespace Api.Mappings;

public sealed class ReservationPassengerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReservationPassenger, ReservationPassengerDto>()
            .Map(dest => dest.ReservationCode, src => src.ReservationFlight.Reservation.ReservationCode.Value)
            .Map(dest => dest.FlightId, src => src.ReservationFlight.FlightId)
            .Map(dest => dest.FlightCode, src => src.ReservationFlight.Flight.FlightCode.Value)
            .Map(dest => dest.DocumentNumber, src => src.Passenger.Person.DocumentNumber.Value)
            .Map(dest => dest.FirstName, src => src.Passenger.Person.FirstName.Value)
            .Map(dest => dest.LastName, src => src.Passenger.Person.LastName.Value);

        config.NewConfig<CreateReservationPassengerRequest, CreateReservationPassenger>();
        config.NewConfig<UpdateReservationPassengerRequest, UpdateReservationPassenger>()
            .Map(dest => dest.Id, _ => 0);
    }
}

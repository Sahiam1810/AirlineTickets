using Api.Dtos.Tickets;
using Application.UseCase.Tickets;
using Domain.Entities.Tickets;
using Mapster;

namespace Api.Mappings;

public sealed class TicketMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Ticket, TicketDto>()
            .Map(dest => dest.ReservationFlightId, src => src.ReservationPassenger.ReservationFlightId)
            .Map(dest => dest.ReservationCode, src => src.ReservationPassenger.ReservationFlight.Reservation.ReservationCode.Value)
            .Map(dest => dest.FlightId, src => src.ReservationPassenger.ReservationFlight.FlightId)
            .Map(dest => dest.FlightCode, src => src.ReservationPassenger.ReservationFlight.Flight.FlightCode.Value)
            .Map(dest => dest.PassengerId, src => src.ReservationPassenger.PassengerId)
            .Map(dest => dest.DocumentNumber, src => src.ReservationPassenger.Passenger.Person.DocumentNumber.Value)
            .Map(dest => dest.FirstName, src => src.ReservationPassenger.Passenger.Person.FirstName.Value)
            .Map(dest => dest.LastName, src => src.ReservationPassenger.Passenger.Person.LastName.Value)
            .Map(dest => dest.TicketStatusName, src => src.TicketStatus.Name);

        config.NewConfig<CreateTicketRequest, CreateTicket>();
        config.NewConfig<UpdateTicketRequest, UpdateTicket>()
            .Map(dest => dest.Id, _ => 0);
    }
}

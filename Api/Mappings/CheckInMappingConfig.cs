using Api.Dtos.CheckIns;
using Application.UseCase.CheckIns;
using Domain.Entities.Tickets;
using Mapster;

namespace Api.Mappings;

public sealed class CheckInMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CheckIn, CheckInDto>()
            .Map(dest => dest.TicketCode, src => src.Ticket.TicketCode)
            .Map(dest => dest.StaffName, src => src.Staff.Person.FirstName.Value + " " + src.Staff.Person.LastName.Value)
            .Map(dest => dest.SeatCode, src => src.FlightSeat.SeatCode.Value)
            .Map(dest => dest.CheckInStatusName, src => src.CheckInStatus.Name);

        config.NewConfig<CreateCheckInRequest, CreateCheckIn>();
        config.NewConfig<UpdateCheckInRequest, UpdateCheckIn>()
            .Map(dest => dest.Id, _ => 0);
    }
}

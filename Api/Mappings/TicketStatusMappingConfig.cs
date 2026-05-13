using Api.Dtos.TicketStatuses;
using Application.UseCase.TicketStatuses;
using Domain.Entities.Tickets;
using Mapster;

namespace Api.Mappings;

public sealed class TicketStatusMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TicketStatus, TicketStatusDto>();

        config.NewConfig<CreateTicketStatusRequest, CreateTicketStatus>();
        config.NewConfig<UpdateTicketStatusRequest, UpdateTicketStatus>()
            .Map(dest => dest.Id, _ => 0);
    }
}

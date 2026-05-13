using Api.Dtos.CheckInStatuses;
using Application.UseCase.CheckInStatuses;
using Domain.Entities.Tickets;
using Mapster;

namespace Api.Mappings;

public sealed class CheckInStatusMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CheckInStatus, CheckInStatusDto>();

        config.NewConfig<CreateCheckInStatusRequest, CreateCheckInStatus>();
        config.NewConfig<UpdateCheckInStatusRequest, UpdateCheckInStatus>()
            .Map(dest => dest.Id, _ => 0);
    }
}

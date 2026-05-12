using Api.Dtos.AvailabilityStatuses;
using Application.UseCase.AvailabilityStatuses;
using Domain.Entities.Staff;
using Mapster;

namespace Api.Mappings;

public sealed class AvailabilityStatusMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AvailabilityStatus, AvailabilityStatusDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateAvailabilityStatusRequest, CreateAvailabilityStatus>();
        config.NewConfig<UpdateAvailabilityStatusRequest, UpdateAvailabilityStatus>()
            .Map(dest => dest.Id, _ => 0);
    }
}

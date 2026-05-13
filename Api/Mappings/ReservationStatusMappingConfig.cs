using Api.Dtos.ReservationStatuses;
using Application.UseCase.ReservationStatuses;
using Domain.Entities.Reservations;
using Mapster;

namespace Api.Mappings;

public sealed class ReservationStatusMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReservationStatus, ReservationStatusDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateReservationStatusRequest, CreateReservationStatus>();
        config.NewConfig<UpdateReservationStatusRequest, UpdateReservationStatus>()
            .Map(dest => dest.Id, _ => 0);
    }
}

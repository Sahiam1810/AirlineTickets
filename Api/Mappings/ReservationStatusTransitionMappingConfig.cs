using Api.Dtos.ReservationStatusTransitions;
using Application.UseCase.ReservationStatusTransitions;
using Domain.Entities.Reservations;
using Mapster;

namespace Api.Mappings;

public sealed class ReservationStatusTransitionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReservationStatusTransition, ReservationStatusTransitionDto>()
            .Map(dest => dest.FromStatusName, src => src.FromStatus.Name.Value)
            .Map(dest => dest.ToStatusName, src => src.ToStatus.Name.Value);

        config.NewConfig<CreateReservationStatusTransitionRequest, CreateReservationStatusTransition>();
    }
}

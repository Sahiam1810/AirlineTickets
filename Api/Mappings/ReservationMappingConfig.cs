using Api.Dtos.Reservations;
using Application.UseCase.Reservations;
using Domain.Entities.Reservations;
using Mapster;

namespace Api.Mappings;

public sealed class ReservationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Reservation, ReservationDto>()
            .Map(dest => dest.ReservationCode, src => src.ReservationCode.Value)
            .Map(dest => dest.TotalValue, src => src.TotalValue.Value);

        config.NewConfig<CreateReservationRequest, CreateReservation>();
        config.NewConfig<UpdateReservationRequest, UpdateReservation>()
            .Map(dest => dest.Id, _ => 0);
    }
}

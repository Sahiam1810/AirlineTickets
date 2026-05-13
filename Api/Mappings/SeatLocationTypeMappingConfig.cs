using Api.Dtos.SeatLocationTypes;
using Application.UseCase.SeatLocationTypes;
using Domain.Entities.Flights;
using Mapster;

namespace Api.Mappings;

public sealed class SeatLocationTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SeatLocationType, SeatLocationTypeDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateSeatLocationTypeRequest, CreateSeatLocationType>();
        config.NewConfig<UpdateSeatLocationTypeRequest, UpdateSeatLocationType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

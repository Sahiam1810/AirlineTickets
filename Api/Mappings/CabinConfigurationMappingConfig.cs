using Api.Dtos.CabinConfigurations;
using Application.UseCase.CabinConfigurations;
using Domain.Entities.Aircraft;
using Mapster;

namespace Api.Mappings;

public sealed class CabinConfigurationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CabinConfiguration, CabinConfigurationDto>()
            .Map(dest => dest.AircraftRegistration, src => src.Aircraft.Registration.Value)
            .Map(dest => dest.CabinTypeName, src => src.CabinType.Name.Value)
            .Map(dest => dest.RowStart, src => src.RowStart.Value)
            .Map(dest => dest.RowEnd, src => src.RowEnd.Value)
            .Map(dest => dest.SeatsPerRow, src => src.SeatsPerRow.Value)
            .Map(dest => dest.SeatLetters, src => src.SeatLetters.Value);

        config.NewConfig<CreateCabinConfigurationRequest, CreateCabinConfiguration>();
        config.NewConfig<UpdateCabinConfigurationRequest, UpdateCabinConfiguration>()
            .Map(dest => dest.Id, _ => 0);
    }
}

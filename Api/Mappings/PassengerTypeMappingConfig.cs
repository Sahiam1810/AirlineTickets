using Api.Dtos.PassengerTypes;
using Application.UseCase.PassengerTypes;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class PassengerTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PassengerType, PassengerTypeDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.AgeMin, src => src.AgeMin == null ? (int?)null : src.AgeMin.Value)
            .Map(dest => dest.AgeMax, src => src.AgeMax == null ? (int?)null : src.AgeMax.Value);

        config.NewConfig<CreatePassengerTypeRequest, CreatePassengerType>();
        config.NewConfig<UpdatePassengerTypeRequest, UpdatePassengerType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

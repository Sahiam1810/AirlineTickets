using Api.Dtos.CabinTypes;
using Application.UseCase.CabinTypes;
using Domain.Entities.Aircraft;
using Mapster;

namespace Api.Mappings;

public sealed class CabinTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CabinType, CabinTypeDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateCabinTypeRequest, CreateCabinType>();
        config.NewConfig<UpdateCabinTypeRequest, UpdateCabinType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

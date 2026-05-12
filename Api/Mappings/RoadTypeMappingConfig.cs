using Api.Dtos.RoadTypes;
using Application.UseCase.RoadTypes;
using Domain.Entities.Location;
using Mapster;

namespace Api.Mappings;

public sealed class RoadTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RoadType, RoadTypeDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateRoadTypeRequest, CreateRoadType>();
        config.NewConfig<UpdateRoadTypeRequest, UpdateRoadType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

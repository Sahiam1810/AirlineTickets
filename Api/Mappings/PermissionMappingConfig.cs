using Api.Dtos.Permissions;
using Application.UseCase.Permissions;
using Domain.Entities.Auth;
using Mapster;

namespace Api.Mappings;

public sealed class PermissionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Permission, PermissionDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreatePermissionRequest, CreatePermission>();
        config.NewConfig<UpdatePermissionRequest, UpdatePermission>()
            .Map(dest => dest.Id, _ => 0);
    }
}

using Api.Dtos.SystemRoles;
using Application.UseCase.SystemRoles;
using Domain.Entities.Auth;
using Mapster;

namespace Api.Mappings;

public sealed class SystemRoleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SystemRole, SystemRoleDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateSystemRoleRequest, CreateSystemRole>();
        config.NewConfig<UpdateSystemRoleRequest, UpdateSystemRole>()
            .Map(dest => dest.Id, _ => 0);
    }
}

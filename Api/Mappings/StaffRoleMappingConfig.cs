using Api.Dtos.StaffRoles;
using Application.UseCase.StaffRoles;
using Domain.Entities.Staff;
using Mapster;

namespace Api.Mappings;

public sealed class StaffRoleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StaffRole, StaffRoleDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateStaffRoleRequest, CreateStaffRole>();
        config.NewConfig<UpdateStaffRoleRequest, UpdateStaffRole>()
            .Map(dest => dest.Id, _ => 0);
    }
}

using Api.Dtos.StaffAvailabilities;
using Application.UseCase.StaffAvailabilities;
using Domain.Entities.Staff;
using Mapster;

namespace Api.Mappings;

public sealed class StaffAvailabilityMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StaffAvailability, StaffAvailabilityDto>()
            .Map(dest => dest.StaffName, src => src.Staff.Person.FirstName.Value + " " + src.Staff.Person.LastName.Value)
            .Map(dest => dest.AvailabilityStatusName, src => src.AvailabilityStatus.Name.Value)
            .Map(dest => dest.Notes, src => src.Notes == null ? null : src.Notes.Value);

        config.NewConfig<CreateStaffAvailabilityRequest, CreateStaffAvailability>();
        config.NewConfig<UpdateStaffAvailabilityRequest, UpdateStaffAvailability>()
            .Map(dest => dest.Id, _ => 0);
    }
}

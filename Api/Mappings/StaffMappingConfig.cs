using Api.Dtos.Staff;
using Application.UseCase.Staff;
using Domain.Entities.Staff;
using Mapster;

namespace Api.Mappings;

public sealed class StaffMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StaffMember, StaffDto>()
            .Map(dest => dest.DocumentNumber, src => src.Person.DocumentNumber.Value)
            .Map(dest => dest.FirstName, src => src.Person.FirstName.Value)
            .Map(dest => dest.LastName, src => src.Person.LastName.Value)
            .Map(dest => dest.StaffRoleName, src => src.StaffRole.Name.Value)
            .Map(dest => dest.AirlineName, src => src.Airline == null ? null : src.Airline.Name.Value)
            .Map(dest => dest.AirportName, src => src.Airport == null ? null : src.Airport.Name.Value)
            .Map(dest => dest.HireDate, src => src.HireDate.Value);

        config.NewConfig<CreateStaffRequest, CreateStaff>();
        config.NewConfig<UpdateStaffRequest, UpdateStaff>()
            .Map(dest => dest.Id, _ => 0);
    }
}

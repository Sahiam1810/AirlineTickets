using Api.Dtos.FlightAssignments;
using Application.UseCase.FlightAssignments;
using Domain.Entities.Flights;
using Mapster;

namespace Api.Mappings;

public sealed class FlightAssignmentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlightAssignment, FlightAssignmentDto>()
            .Map(dest => dest.FlightCode, src => src.Flight.FlightCode.Value)
            .Map(dest => dest.PersonId, src => src.Staff.PersonId)
            .Map(dest => dest.DocumentNumber, src => src.Staff.Person.DocumentNumber.Value)
            .Map(dest => dest.FirstName, src => src.Staff.Person.FirstName.Value)
            .Map(dest => dest.LastName, src => src.Staff.Person.LastName.Value)
            .Map(dest => dest.FlightRoleName, src => src.FlightRole.Name.Value);

        config.NewConfig<CreateFlightAssignmentRequest, CreateFlightAssignment>();
        config.NewConfig<UpdateFlightAssignmentRequest, UpdateFlightAssignment>()
            .Map(dest => dest.Id, _ => 0);
    }
}

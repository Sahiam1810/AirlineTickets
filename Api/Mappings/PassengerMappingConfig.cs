using Api.Dtos.Passengers;
using Application.UseCase.Passengers;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class PassengerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Passenger, PassengerDto>()
            .Map(dest => dest.DocumentNumber, src => src.Person.DocumentNumber.Value)
            .Map(dest => dest.FirstName, src => src.Person.FirstName.Value)
            .Map(dest => dest.LastName, src => src.Person.LastName.Value)
            .Map(dest => dest.PassengerTypeName, src => src.PassengerType.Name.Value);

        config.NewConfig<CreatePassengerRequest, CreatePassenger>();
        config.NewConfig<UpdatePassengerRequest, UpdatePassenger>()
            .Map(dest => dest.Id, _ => 0);
    }
}

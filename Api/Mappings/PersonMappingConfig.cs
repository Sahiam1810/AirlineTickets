using Api.Dtos.People;
using Application.UseCase.People;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class PersonMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Person, PersonDto>()
            .Map(dest => dest.DocumentTypeId, src => src.DocumentTypeId)
            .Map(dest => dest.DocumentNumber, src => src.DocumentNumber.Value)
            .Map(dest => dest.FirstName, src => src.FirstName.Value)
            .Map(dest => dest.LastName, src => src.LastName.Value)
            .Map(dest => dest.BirthDate, src => src.BirthDate)
            .Map(dest => dest.Gender, src => src.Gender == null ? null : src.Gender.Value)
            .Map(dest => dest.AddressId, src => src.AddressId);

        config.NewConfig<CreatePersonRequest, CreatePerson>();
        config.NewConfig<UpdatePersonRequest, UpdatePerson>()
            .Map(dest => dest.Id, _ => 0);
    }
}

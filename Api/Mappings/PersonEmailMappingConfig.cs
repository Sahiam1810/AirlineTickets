using Api.Dtos.PersonEmails;
using Application.UseCase.PersonEmails;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class PersonEmailMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PersonEmail, PersonEmailDto>()
            .Map(dest => dest.EmailUser, src => src.EmailUser.Value)
            .Map(dest => dest.Domain, src => src.EmailDomain.Domain.Value)
            .Map(dest => dest.Email, src => $"{src.EmailUser.Value}@{src.EmailDomain.Domain.Value}");

        config.NewConfig<CreatePersonEmailRequest, CreatePersonEmail>();
        config.NewConfig<UpdatePersonEmailRequest, UpdatePersonEmail>()
            .Map(dest => dest.Id, _ => 0);
    }
}

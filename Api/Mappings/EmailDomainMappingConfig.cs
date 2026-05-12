using Api.Dtos.EmailDomains;
using Application.UseCase.EmailDomains;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class EmailDomainMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EmailDomain, EmailDomainDto>()
            .Map(dest => dest.Domain, src => src.Domain.Value);

        config.NewConfig<CreateEmailDomainRequest, CreateEmailDomain>();
        config.NewConfig<UpdateEmailDomainRequest, UpdateEmailDomain>()
            .Map(dest => dest.Id, _ => 0);
    }
}

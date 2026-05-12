using Api.Dtos.PersonPhones;
using Application.UseCase.PersonPhones;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class PersonPhoneMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PersonPhone, PersonPhoneDto>()
            .Map(dest => dest.CountryCode, src => src.PhoneCode.CountryCode.Value)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber.Value)
            .Map(dest => dest.Phone, src => $"{src.PhoneCode.CountryCode.Value}{src.PhoneNumber.Value}");

        config.NewConfig<CreatePersonPhoneRequest, CreatePersonPhone>();
        config.NewConfig<UpdatePersonPhoneRequest, UpdatePersonPhone>()
            .Map(dest => dest.Id, _ => 0);
    }
}

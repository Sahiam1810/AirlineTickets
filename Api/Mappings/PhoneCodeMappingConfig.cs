using Api.Dtos.PhoneCodes;
using Application.UseCase.PhoneCodes;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class PhoneCodeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PhoneCode, PhoneCodeDto>()
            .Map(dest => dest.CountryCode, src => src.CountryCode.Value)
            .Map(dest => dest.CountryName, src => src.CountryName.Value);

        config.NewConfig<CreatePhoneCodeRequest, CreatePhoneCode>();
        config.NewConfig<UpdatePhoneCodeRequest, UpdatePhoneCode>()
            .Map(dest => dest.Id, _ => 0);
    }
}

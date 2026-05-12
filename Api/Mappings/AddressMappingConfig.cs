using Api.Dtos.Addresses;
using Application.UseCase.Addresses;
using Domain.Entities.Location;
using Mapster;

namespace Api.Mappings;

public sealed class AddressMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressDto>()
            .Map(dest => dest.RoadTypeId, src => src.RoadTypeId)
            .Map(dest => dest.StreetName, src => src.StreetName.Value)
            .Map(dest => dest.Number, src => src.Number == null ? null : src.Number.Value)
            .Map(dest => dest.Complement, src => src.Complement == null ? null : src.Complement.Value)
            .Map(dest => dest.CityId, src => src.CityId)
            .Map(dest => dest.PostalCode, src => src.PostalCode == null ? null : src.PostalCode.Value);

        config.NewConfig<CreateAddressRequest, CreateAddress>();
        config.NewConfig<UpdateAddressRequest, UpdateAddress>()
            .Map(dest => dest.Id, _ => 0);
    }
}

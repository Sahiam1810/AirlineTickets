using Api.Dtos.Clients;
using Application.UseCase.Clients;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class ClientMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Client, ClientDto>()
            .Map(dest => dest.DocumentNumber, src => src.Person.DocumentNumber.Value)
            .Map(dest => dest.FirstName, src => src.Person.FirstName.Value)
            .Map(dest => dest.LastName, src => src.Person.LastName.Value);

        config.NewConfig<CreateClientRequest, CreateClient>();
    }
}

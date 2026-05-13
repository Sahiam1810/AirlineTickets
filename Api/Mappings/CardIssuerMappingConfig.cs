using Api.Dtos.CardIssuers;
using Application.UseCase.CardIssuers;
using Domain.Entities.Payments;
using Mapster;

namespace Api.Mappings;

public sealed class CardIssuerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CardIssuer, CardIssuerDto>();

        config.NewConfig<CreateCardIssuerRequest, CreateCardIssuer>();

        config.NewConfig<UpdateCardIssuerRequest, UpdateCardIssuer>()
            .Map(dest => dest.Id, _ => 0);
    }
}

using Api.Dtos.CardTypes;
using Application.UseCase.CardTypes;
using Domain.Entities.Payments;
using Mapster;

namespace Api.Mappings;

public sealed class CardTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CardType, CardTypeDto>();

        config.NewConfig<CreateCardTypeRequest, CreateCardType>();

        config.NewConfig<UpdateCardTypeRequest, UpdateCardType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

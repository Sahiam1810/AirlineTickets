using Api.Dtos.PaymentMethodTypes;
using Application.UseCase.PaymentMethodTypes;
using Domain.Entities.Payments;
using Mapster;

namespace Api.Mappings;

public sealed class PaymentMethodTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentMethodType, PaymentMethodTypeDto>();

        config.NewConfig<CreatePaymentMethodTypeRequest, CreatePaymentMethodType>();

        config.NewConfig<UpdatePaymentMethodTypeRequest, UpdatePaymentMethodType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

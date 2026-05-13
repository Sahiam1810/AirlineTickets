using Api.Dtos.PaymentStates;
using Application.UseCase.PaymentStates;
using Domain.Entities.Payments;
using Mapster;

namespace Api.Mappings;

public sealed class PaymentStateMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentState, PaymentStateDto>();

        config.NewConfig<CreatePaymentStateRequest, CreatePaymentState>();
        
        config.NewConfig<UpdatePaymentStateRequest, UpdatePaymentState>()
            .Map(dest => dest.Id, _ => 0);
    }
}

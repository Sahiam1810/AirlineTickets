using MediatR;

namespace Application.UseCase.PaymentMethods;

public sealed record CreatePaymentMethod(
    int PaymentMethodTypeId,
    int? CardTypeId,
    int? CardIssuerId,
    string CommercialName) : IRequest<int>;

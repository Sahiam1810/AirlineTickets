using MediatR;

namespace Application.UseCase.PaymentMethods;

public sealed record UpdatePaymentMethod(
    int Id,
    int PaymentMethodTypeId,
    int? CardTypeId,
    int? CardIssuerId,
    string CommercialName) : IRequest;

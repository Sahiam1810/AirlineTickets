using MediatR;

namespace Application.UseCase.PaymentMethodTypes;

public sealed record CreatePaymentMethodType(string Name) : IRequest<int>;

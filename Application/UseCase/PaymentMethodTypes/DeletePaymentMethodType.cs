using MediatR;

namespace Application.UseCase.PaymentMethodTypes;

public sealed record DeletePaymentMethodType(int Id) : IRequest;

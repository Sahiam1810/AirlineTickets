using MediatR;

namespace Application.UseCase.PaymentMethodTypes;

public sealed record UpdatePaymentMethodType(int Id, string Name) : IRequest;

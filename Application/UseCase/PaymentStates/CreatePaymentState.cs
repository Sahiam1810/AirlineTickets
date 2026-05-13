using MediatR;

namespace Application.UseCase.PaymentStates;

public sealed record CreatePaymentState(string Name) : IRequest<int>;

using MediatR;

namespace Application.UseCase.PaymentStates;

public sealed record DeletePaymentState(int Id) : IRequest;

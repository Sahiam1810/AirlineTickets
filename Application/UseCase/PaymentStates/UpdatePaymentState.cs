using MediatR;

namespace Application.UseCase.PaymentStates;

public sealed record UpdatePaymentState(int Id, string Name) : IRequest;

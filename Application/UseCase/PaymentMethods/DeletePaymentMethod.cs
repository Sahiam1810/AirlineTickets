using MediatR;

namespace Application.UseCase.PaymentMethods;

public sealed record DeletePaymentMethod(int Id) : IRequest;

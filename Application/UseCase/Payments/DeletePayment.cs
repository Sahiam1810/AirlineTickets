using MediatR;

namespace Application.UseCase.Payments;

public sealed record DeletePayment(int Id) : IRequest;

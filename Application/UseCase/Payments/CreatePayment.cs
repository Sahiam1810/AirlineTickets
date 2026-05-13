using System;
using MediatR;

namespace Application.UseCase.Payments;

public sealed record CreatePayment(
    int ReservationId,
    decimal Amount,
    DateTime PaymentDate,
    int PaymentStateId,
    int PaymentMethodId) : IRequest<int>;

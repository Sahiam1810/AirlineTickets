using System;
using MediatR;

namespace Application.UseCase.Payments;

public sealed record UpdatePayment(
    int Id,
    int ReservationId,
    decimal Amount,
    DateTime PaymentDate,
    int PaymentStateId,
    int PaymentMethodId) : IRequest;

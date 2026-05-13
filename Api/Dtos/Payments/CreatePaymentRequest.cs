using System;

namespace Api.Dtos.Payments;

public sealed class CreatePaymentRequest
{
    public int ReservationId { get; init; }
    public decimal Amount { get; init; }
    public DateTime PaymentDate { get; init; }
    public int PaymentStateId { get; init; }
    public int PaymentMethodId { get; init; }
}

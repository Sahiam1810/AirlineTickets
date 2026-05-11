using System;
using Domain.Common;
using Domain.Entities.Reservations;

namespace Domain.Entities.Payments;

public sealed class Payment : BaseEntity<int>
{
    public int ReservationId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public int PaymentStateId { get; private set; }
    public int PaymentMethodId { get; private set; }
    
    // Navigation
    public Reservation Reservation { get; set; } = null!;
    public PaymentState PaymentState { get; set; } = null!;
    public PaymentMethod PaymentMethod { get; set; } = null!;

    private Payment() { }

    public Payment(int reservationId, decimal amount, DateTime paymentDate,
        int paymentStateId, int paymentMethodId)
    {
        ReservationId = reservationId;
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentStateId = paymentStateId;
        PaymentMethodId = paymentMethodId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateState(int paymentStateId)
    {
        PaymentStateId = paymentStateId;
        UpdatedAt = DateTime.UtcNow;
    }
}
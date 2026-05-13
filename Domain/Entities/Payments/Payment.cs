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
        Validate(reservationId, amount, paymentDate, paymentStateId, paymentMethodId);

        ReservationId = reservationId;
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentStateId = paymentStateId;
        PaymentMethodId = paymentMethodId;
    }

    public void Update(int reservationId, decimal amount, DateTime paymentDate,
        int paymentStateId, int paymentMethodId)
    {
        Validate(reservationId, amount, paymentDate, paymentStateId, paymentMethodId);

        ReservationId = reservationId;
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentStateId = paymentStateId;
        PaymentMethodId = paymentMethodId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateState(int paymentStateId)
    {
        if (paymentStateId <= 0)
        {
            throw new ArgumentException("Payment state id must be greater than zero.");
        }

        PaymentStateId = paymentStateId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int reservationId, decimal amount, DateTime paymentDate,
        int paymentStateId, int paymentMethodId)
    {
        if (reservationId <= 0)
        {
            throw new ArgumentException("Reservation id must be greater than zero.");
        }

        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative.");
        }

        if (paymentDate == default)
        {
            throw new ArgumentException("Payment date is required.");
        }

        if (paymentStateId <= 0)
        {
            throw new ArgumentException("Payment state id must be greater than zero.");
        }

        if (paymentMethodId <= 0)
        {
            throw new ArgumentException("Payment method id must be greater than zero.");
        }
    }
}

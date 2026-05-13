using Domain.Common;
using Domain.Entities.Reservations;

namespace Domain.Entities.Payments;

public sealed class Invoice : BaseEntity<int>
{
    public int ReservationId { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public DateTime IssuedAt { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal Taxes { get; private set; }
    public decimal Total { get; private set; }

    public Reservation Reservation { get; set; } = null!;

    private Invoice() { }

    public Invoice(int reservationId, string invoiceNumber, DateTime issuedAt, decimal subtotal, decimal taxes, decimal total)
    {
        SetValues(reservationId, invoiceNumber, issuedAt, subtotal, taxes, total);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(int reservationId, string invoiceNumber, DateTime issuedAt, decimal subtotal, decimal taxes, decimal total)
    {
        SetValues(reservationId, invoiceNumber, issuedAt, subtotal, taxes, total);
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetValues(int reservationId, string invoiceNumber, DateTime issuedAt, decimal subtotal, decimal taxes, decimal total)
    {
        if (reservationId <= 0)
        {
            throw new ArgumentException("Reservation ID must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(invoiceNumber))
        {
            throw new ArgumentException("Invoice number is required.");
        }

        invoiceNumber = invoiceNumber.Trim();

        if (invoiceNumber.Length > 30)
        {
            throw new ArgumentException("Invoice number cannot exceed 30 characters.");
        }

        if (issuedAt == default)
        {
            throw new ArgumentException("Issued at is required.");
        }

        if (subtotal < 0)
        {
            throw new ArgumentException("Subtotal cannot be negative.");
        }

        if (taxes < 0)
        {
            throw new ArgumentException("Taxes cannot be negative.");
        }

        if (total < 0)
        {
            throw new ArgumentException("Total cannot be negative.");
        }

        if (total != subtotal + taxes)
        {
            throw new ArgumentException("Total must be equal to subtotal plus taxes.");
        }

        ReservationId = reservationId;
        InvoiceNumber = invoiceNumber;
        IssuedAt = issuedAt;
        Subtotal = subtotal;
        Taxes = taxes;
        Total = total;
    }
}

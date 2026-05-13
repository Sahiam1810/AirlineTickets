namespace Api.Dtos.Invoices;

public sealed class UpdateInvoiceRequest
{
    public int ReservationId { get; init; }
    public string InvoiceNumber { get; init; } = string.Empty;
    public DateTime IssuedAt { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Taxes { get; init; }
    public decimal Total { get; init; }
}

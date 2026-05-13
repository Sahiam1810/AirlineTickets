namespace Api.Dtos.Invoices;

public sealed class InvoiceDto
{
    public int Id { get; init; }
    public int ReservationId { get; init; }
    public string InvoiceNumber { get; init; } = string.Empty;
    public DateTime IssuedAt { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Taxes { get; init; }
    public decimal Total { get; init; }
    public DateTime CreatedAt { get; init; }
}

namespace Api.Dtos.InvoiceItems;

public sealed class CreateInvoiceItemRequest
{
    public int InvoiceId { get; init; }
    public int InvoiceItemTypeId { get; init; }
    public string Description { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Subtotal { get; init; }
    public int? ReservationPassengerId { get; init; }
}

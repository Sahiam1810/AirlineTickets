using System;

namespace Api.Dtos.InvoiceItems;

public sealed class InvoiceItemDto
{
    public int Id { get; init; }
    public int InvoiceId { get; init; }
    public int InvoiceItemTypeId { get; init; }
    public string Description { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Subtotal { get; init; }
    public int? ReservationPassengerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

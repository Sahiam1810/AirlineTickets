using System;

namespace Api.Dtos.PaymentMethods;

public sealed class PaymentMethodDto
{
    public int Id { get; init; }
    public int PaymentMethodTypeId { get; init; }
    public int? CardTypeId { get; init; }
    public int? CardIssuerId { get; init; }
    public string CommercialName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

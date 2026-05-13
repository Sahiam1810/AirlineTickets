using System;

namespace Api.Dtos.PaymentStates;

public sealed class PaymentStateDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

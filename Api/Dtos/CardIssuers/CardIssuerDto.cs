using System;

namespace Api.Dtos.CardIssuers;

public sealed class CardIssuerDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

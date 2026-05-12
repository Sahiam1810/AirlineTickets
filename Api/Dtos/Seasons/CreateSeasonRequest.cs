namespace Api.Dtos.Seasons;

public sealed class CreateSeasonRequest
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public decimal PriceFactor { get; init; }
}

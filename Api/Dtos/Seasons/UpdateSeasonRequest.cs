namespace Api.Dtos.Seasons;

public sealed class UpdateSeasonRequest
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public decimal PriceFactor { get; init; }
}

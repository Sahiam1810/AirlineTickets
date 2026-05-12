namespace Api.Dtos.Seasons;

public sealed class SeasonDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public decimal PriceFactor { get; init; }
}

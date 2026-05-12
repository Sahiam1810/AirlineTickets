namespace Api.Dtos.Regions;

public sealed class UpdateRegionRequest
{
    public string Name { get; init; } = default!;
    public string Type { get; init; } = default!;
    public int CountryId { get; init; }
}

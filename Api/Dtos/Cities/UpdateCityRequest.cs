namespace Api.Dtos.Cities;

public sealed class UpdateCityRequest
{
    public string Name { get; init; } = default!;
    public int RegionId { get; init; }
}

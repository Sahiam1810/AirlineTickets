namespace Api.Dtos.Cities;

public sealed class CreateCityRequest
{
    public string Name { get; init; } = default!;
    public int RegionId { get; init; }
}

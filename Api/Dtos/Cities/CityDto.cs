namespace Api.Dtos.Cities;

public sealed class CityDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public int RegionId { get; init; }
}

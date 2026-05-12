namespace Api.Dtos.Countries;

public sealed class CountryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public int ContinentId { get; init; }
}

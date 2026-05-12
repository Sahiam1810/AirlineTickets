namespace Api.Dtos.Countries;

public sealed class CreateCountryRequest
{
    public string Name { get; init; } = default!;
    public int ContinentId { get; init; }
}

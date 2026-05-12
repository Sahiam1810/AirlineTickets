namespace Api.Dtos.Countries;

public sealed class UpdateCountryRequest
{
    public string Name { get; init; } = default!;
    public int ContinentId { get; init; }
}

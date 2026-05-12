namespace Api.Dtos.Airlines;

public sealed class CreateAirlineRequest
{
    public string Name { get; init; } = default!;
    public string IataCode { get; init; } = default!;
    public int CountryId { get; init; }
    public bool IsActive { get; init; }
}

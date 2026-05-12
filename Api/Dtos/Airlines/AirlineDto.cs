namespace Api.Dtos.Airlines;

public sealed class AirlineDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string IataCode { get; init; } = default!;
    public int CountryId { get; init; }
    public string CountryName { get; init; } = default!;
    public bool IsActive { get; init; }
}

using Domain.Common;
using Domain.Entities.Geography;

namespace Domain.Entities.Airlines;

public sealed class Airport : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string IataCode { get; set; } = string.Empty;
    public string? IcaoCode { get; set; }
    public int CityId { get; set; }

    // Navigation
    public City City { get; set; } = null!;
    public ICollection<AirportAirline> AirportAirlines { get; set; } = [];
}
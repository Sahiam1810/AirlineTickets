using Domain.Common;
using Domain.Entities.Geography;

namespace Domain.Entities.Airlines;

public sealed class Airline : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string IataCode { get; set; } = string.Empty;
    public int OriginCountryId { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public Country OriginCountry { get; set; } = null!;
    public ICollection<AirportAirline> AirportAirlines { get; set; } = [];
}
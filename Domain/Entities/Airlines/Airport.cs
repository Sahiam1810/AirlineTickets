using Domain.Common;
using Domain.Entities.Geography;
using Domain.Entities.Routes;
using Domain.Entities.Staff;
using Domain.ValueObjects.Airports;

namespace Domain.Entities.Airlines;

public sealed class Airport : BaseEntity<int>
{
    public AirportName Name { get; private set; } = null!;
    public IataCode IataCode { get; private set; } = null!;
    public IcaoCode? IcaoCode { get; private set; }
    public int CityId { get; private set; }

    private Airport() { }

    public Airport(AirportName name, IataCode iataCode, IcaoCode? icaoCode, int cityId)
    {
        if (cityId <= 0)
            throw new ArgumentException("City id is required", nameof(cityId));

        Name = name;
        IataCode = iataCode;
        IcaoCode = icaoCode;
        CityId = cityId;
    }

    public void Update(AirportName name, IataCode iataCode, IcaoCode? icaoCode, int cityId)
    {
        if (cityId <= 0)
            throw new ArgumentException("City id is required", nameof(cityId));

        Name = name;
        IataCode = iataCode;
        IcaoCode = icaoCode;
        CityId = cityId;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public City City { get; set; } = null!;
    public ICollection<AirportAirline> AirportAirlines { get; set; } = [];
    public ICollection<StaffMember> StaffMembers { get; set; } = [];
    public ICollection<RouteStop> RouteStops { get; set; } = [];
}

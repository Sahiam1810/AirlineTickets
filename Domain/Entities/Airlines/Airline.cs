using Domain.Common;
using Domain.Entities.Geography;
using Domain.ValueObjects.Airlines;

namespace Domain.Entities.Airlines;

public sealed class Airline : BaseEntity<int>
{
    public AirlineName Name { get; private set; } = null!;
    public IataCode IataCode { get; private set; } = null!;
    public int CountryId { get; private set; }
    public bool IsActive { get; private set; }

    private Airline() { }

    public Airline(AirlineName name, IataCode iataCode, int countryId, bool isActive)
    {
        if (countryId <= 0)
            throw new ArgumentException("Country id is required", nameof(countryId));

        Name = name;
        IataCode = iataCode;
        CountryId = countryId;
        IsActive = isActive;
    }

    public void Update(AirlineName name, IataCode iataCode, int countryId, bool isActive)
    {
        if (countryId <= 0)
            throw new ArgumentException("Country id is required", nameof(countryId));

        Name = name;
        IataCode = iataCode;
        CountryId = countryId;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public Country Country { get; set; } = null!;
    public ICollection<AirportAirline> AirportAirlines { get; set; } = [];
}

using System;
using Domain.Common;
using Domain.Entities.Geography;
using Domain.ValueObjects.Location;

namespace Domain.Entities.Location;

public sealed class Address : BaseEntity<int>
{
    public int RoadTypeId { get; private set; }
    public StreetName StreetName { get; private set; } = null!;
    public AddressNumber? Number { get; private set; }
    public AddressComplement? Complement { get; private set; }
    public int CityId { get; private set; }
    public PostalCode? PostalCode { get; private set; }

    public RoadType RoadType { get; set; } = null!;
    public City City { get; set; } = null!;

    private Address() { }

    public Address(
        int roadTypeId,
        string streetName,
        string? number,
        string? complement,
        int cityId,
        string? postalCode)
    {
        RoadTypeId = roadTypeId;
        StreetName = StreetName.Create(streetName);
        Number = AddressNumber.Create(number);
        Complement = AddressComplement.Create(complement);
        CityId = cityId;
        PostalCode = PostalCode.Create(postalCode);
    }

    public void Update(
        int roadTypeId,
        string streetName,
        string? number,
        string? complement,
        int cityId,
        string? postalCode)
    {
        RoadTypeId = roadTypeId;
        StreetName = StreetName.Create(streetName);
        Number = AddressNumber.Create(number);
        Complement = AddressComplement.Create(complement);
        CityId = cityId;
        PostalCode = PostalCode.Create(postalCode);
        UpdatedAt = DateTime.UtcNow;
    }
}

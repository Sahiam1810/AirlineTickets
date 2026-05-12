using System;
using Domain.Common;
using Domain.Entities.Aircraft;
using Domain.Entities.People;
using Domain.ValueObjects.Fares;

namespace Domain.Entities.Routes;

public sealed class Fare : BaseEntity<int>
{
    public int RouteId { get; private set; }
    public int CabinTypeId { get; private set; }
    public int PassengerTypeId { get; private set; }
    public int SeasonId { get; private set; }
    public BasePrice BasePrice { get; private set; } = null!;
    public DateOnly? ValidFrom { get; private set; }
    public DateOnly? ValidTo { get; private set; }

    private Fare() { }

    public Fare(
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        decimal basePrice,
        DateOnly? validFrom,
        DateOnly? validTo)
    {
        ValidateIds(routeId, cabinTypeId, passengerTypeId, seasonId);
        DateRange.Validate(validFrom, validTo);

        RouteId = routeId;
        CabinTypeId = cabinTypeId;
        PassengerTypeId = passengerTypeId;
        SeasonId = seasonId;
        BasePrice = BasePrice.Create(basePrice);
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public void Update(
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        decimal basePrice,
        DateOnly? validFrom,
        DateOnly? validTo)
    {
        ValidateIds(routeId, cabinTypeId, passengerTypeId, seasonId);
        DateRange.Validate(validFrom, validTo);

        RouteId = routeId;
        CabinTypeId = cabinTypeId;
        PassengerTypeId = passengerTypeId;
        SeasonId = seasonId;
        BasePrice = BasePrice.Create(basePrice);
        ValidFrom = validFrom;
        ValidTo = validTo;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateIds(int routeId, int cabinTypeId, int passengerTypeId, int seasonId)
    {
        if (routeId <= 0)
            throw new ArgumentException("Route id is required", nameof(routeId));

        if (cabinTypeId <= 0)
            throw new ArgumentException("Cabin type id is required", nameof(cabinTypeId));

        if (passengerTypeId <= 0)
            throw new ArgumentException("Passenger type id is required", nameof(passengerTypeId));

        if (seasonId <= 0)
            throw new ArgumentException("Season id is required", nameof(seasonId));
    }

    // Navigation
    public Route Route { get; set; } = null!;
    public CabinType CabinType { get; set; } = null!;
    public PassengerType PassengerType { get; set; } = null!;
    public Season Season { get; set; } = null!;
}

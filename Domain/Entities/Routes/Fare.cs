using System;
using Domain.Common;
using Domain.Entities.Aircraft;
using Domain.Entities.People;

namespace Domain.Entities.Routes;

public sealed class Fare : BaseEntity<int>
{
    public int RouteId { get; set; }
    public int CabinTypeId { get; set; }
    public int PassengerTypeId { get; set; }
    public int SeasonId { get; set; }
    public decimal BasePrice { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidUntil { get; set; }

    // Navigation
    public Route Route { get; set; } = null!;
    public CabinType CabinType { get; set; } = null!;
    public PassengerType PassengerType { get; set; } = null!;
    public Season Season { get; set; } = null!;
}
using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class Passenger : BaseEntity<int>
{
    public int PersonId { get; set; }
    public int PassengerTypeId { get; set; }

    // Navigation
    public Person Person { get; set; } = null!;
    public PassengerType PassengerType { get; set; } = null!;
}
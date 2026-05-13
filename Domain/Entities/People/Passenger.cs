using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class Passenger : BaseEntity<int>
{
    public int PersonId { get; private set; }
    public int PassengerTypeId { get; private set; }

    private Passenger() { }

    public Passenger(int personId, int passengerTypeId)
    {
        ValidateIds(personId, passengerTypeId);

        PersonId = personId;
        PassengerTypeId = passengerTypeId;
    }

    public void Update(int passengerTypeId)
    {
        if (passengerTypeId <= 0)
            throw new ArgumentException("Passenger type id is required", nameof(passengerTypeId));

        PassengerTypeId = passengerTypeId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateIds(int personId, int passengerTypeId)
    {
        if (personId <= 0)
            throw new ArgumentException("Person id is required", nameof(personId));

        if (passengerTypeId <= 0)
            throw new ArgumentException("Passenger type id is required", nameof(passengerTypeId));
    }

    // Navigation
    public Person Person { get; set; } = null!;
    public PassengerType PassengerType { get; set; } = null!;
}

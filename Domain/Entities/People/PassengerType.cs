using System;
using Domain.Common;
using Domain.ValueObjects.PassengerTypes;

namespace Domain.Entities.People;

public sealed class PassengerType : BaseEntity<int>
{
    public PassengerTypeName Name { get; private set; } = null!;
    public Age? AgeMin { get; private set; }
    public Age? AgeMax { get; private set; }

    private PassengerType() { }

    public PassengerType(string name, int? ageMin, int? ageMax)
    {
        Name = PassengerTypeName.Create(name);
        AgeMin = Age.Create(ageMin);
        AgeMax = Age.Create(ageMax);
        ValidateAgeRange(AgeMin, AgeMax);
    }

    public void Update(string name, int? ageMin, int? ageMax)
    {
        var min = Age.Create(ageMin);
        var max = Age.Create(ageMax);

        ValidateAgeRange(min, max);

        Name = PassengerTypeName.Create(name);
        AgeMin = min;
        AgeMax = max;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateAgeRange(Age? ageMin, Age? ageMax)
    {
        if (ageMin is not null && ageMax is not null && ageMin.Value > ageMax.Value)
            throw new ArgumentException("La edad minima no puede ser mayor que la edad maxima");
    }

    // Navigation
    public ICollection<Passenger> Passengers { get; set; } = [];
}

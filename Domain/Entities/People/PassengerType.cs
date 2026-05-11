using System;

namespace Domain.Entities.People;

public sealed class PassengerType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }

    // Navigation
    public ICollection<Passenger> Passengers { get; set; } = [];
}
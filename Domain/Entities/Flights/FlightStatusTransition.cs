using System;
using Domain.Common;
using Domain.ValueObjects.FlightStatusTransitions;

namespace Domain.Entities.Flights;

public sealed class FlightStatusTransition : BaseEntity<int>
{
    public int FromStateId { get; private set; }
    public int ToStateId { get; private set; }

    private FlightStatusTransition() { }

    public FlightStatusTransition(int fromStateId, int toStateId)
    {
        Validate(fromStateId, toStateId);

        FromStateId = fromStateId;
        ToStateId = toStateId;
    }

    public void Update(int fromStateId, int toStateId)
    {
        Validate(fromStateId, toStateId);

        FromStateId = fromStateId;
        ToStateId = toStateId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(int fromStateId, int toStateId)
    {
        _ = StateId.Create(fromStateId);
        _ = StateId.Create(toStateId);

        if (fromStateId == toStateId)
            throw new ArgumentException("El estado origen no puede ser igual al estado destino");
    }

    // Navigation
    public FlightState FromState { get; set; } = null!;
    public FlightState ToState { get; set; } = null!;
}

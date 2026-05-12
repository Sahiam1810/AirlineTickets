using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class Client : BaseEntity<int>
{
    public int PersonId { get; private set; }

    private Client() { }

    public Client(int personId)
    {
        if (personId <= 0)
            throw new ArgumentException("Person id is required", nameof(personId));

        PersonId = personId;
    }

    // Navigation
    public Person Person { get; set; } = null!;
}

using System;
using Domain.Common;

namespace Domain.Entities.People;

public sealed class Client : BaseEntity<int>
{
    public int PersonId { get; set; }

    // Navigation
    public Person Person { get; set; } = null!;
}
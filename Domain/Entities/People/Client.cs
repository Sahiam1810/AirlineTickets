using System;

namespace Domain.Entities.People;

public sealed class Client
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Person Person { get; set; } = null!;
}
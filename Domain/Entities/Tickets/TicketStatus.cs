using System;
using Domain.Common;

namespace Domain.Entities.Tickets;

public sealed class TicketStatus : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    // Navigation
    public ICollection<Ticket> Tickets { get; set; } = [];

    private TicketStatus() { }

    public TicketStatus(string name)
    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
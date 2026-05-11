using System;
using Domain.Common;

namespace Domain.Entities.Routes;

public sealed class Season : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PriceFactor { get; set; }

    // Navigation
    public ICollection<Fare> Fares { get; set; } = [];
}
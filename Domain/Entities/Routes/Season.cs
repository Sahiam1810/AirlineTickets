using System;

namespace Domain.Entities.Routes;

public sealed class Season
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PriceFactor { get; set; }

    // Navigation
    public ICollection<Fare> Fares { get; set; } = [];
}
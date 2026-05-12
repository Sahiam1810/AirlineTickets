using System;
using Domain.Common;
using Domain.ValueObjects.Seasons;

namespace Domain.Entities.Routes;

public sealed class Season : BaseEntity<int>
{
    public SeasonName Name { get; private set; } = null!;
    public SeasonDescription? Description { get; private set; }
    public PriceFactor PriceFactor { get; private set; } = null!;

    private Season() { }

    public Season(string name, string? description, decimal priceFactor)
    {
        Name = SeasonName.Create(name);
        Description = SeasonDescription.Create(description);
        PriceFactor = PriceFactor.Create(priceFactor);
    }

    public void Update(string name, string? description, decimal priceFactor)
    {
        Name = SeasonName.Create(name);
        Description = SeasonDescription.Create(description);
        PriceFactor = PriceFactor.Create(priceFactor);
        UpdatedAt = DateTime.UtcNow;
    }

    // Navigation
    public ICollection<Fare> Fares { get; set; } = [];
}

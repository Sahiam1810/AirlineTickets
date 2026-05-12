using System;

namespace Api.Dtos.Continents;

public sealed class ContinentDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
}
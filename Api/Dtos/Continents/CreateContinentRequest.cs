using System;

namespace Api.Dtos.Continents;

public sealed class CreateContinentRequest
{
    public string Name { get; init; } = default!;
}
using System;
using FluentValidation;

namespace Application.UseCase.Continents;

public sealed class CreateContinentValidator : AbstractValidator<CreateContinent>
{
    public CreateContinentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
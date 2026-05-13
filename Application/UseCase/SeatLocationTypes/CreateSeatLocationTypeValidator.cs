using FluentValidation;

namespace Application.UseCase.SeatLocationTypes;

public sealed class CreateSeatLocationTypeValidator : AbstractValidator<CreateSeatLocationType>
{
    public CreateSeatLocationTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Trim().All(char.IsLetter))
            .WithMessage("El nombre solo puede contener letras.");
    }
}

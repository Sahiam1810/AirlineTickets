using FluentValidation;

namespace Application.UseCase.SeatLocationTypes;

public sealed class UpdateSeatLocationTypeValidator : AbstractValidator<UpdateSeatLocationType>
{
    public UpdateSeatLocationTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Trim().All(char.IsLetter))
            .WithMessage("El nombre solo puede contener letras.");
    }
}

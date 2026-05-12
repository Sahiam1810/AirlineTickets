using FluentValidation;

namespace Application.UseCase.Seasons;

public sealed class CreateSeasonValidator : AbstractValidator<CreateSeason>
{
    public CreateSeasonValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(150);

        RuleFor(x => x.PriceFactor)
            .GreaterThan(0).WithMessage("El factor de precio debe ser mayor que 0.");
    }
}

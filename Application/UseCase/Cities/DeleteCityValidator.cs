using FluentValidation;

namespace Application.UseCase.Cities;

public sealed class DeleteCityValidator : AbstractValidator<DeleteCity>
{
    public DeleteCityValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");
    }
}

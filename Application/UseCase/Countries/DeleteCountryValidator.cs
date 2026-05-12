using FluentValidation;

namespace Application.UseCase.Countries;

public sealed class DeleteCountryValidator : AbstractValidator<DeleteCountry>
{
    public DeleteCountryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");
    }
}

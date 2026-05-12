using FluentValidation;

namespace Application.UseCase.PhoneCodes;

public sealed class CreatePhoneCodeValidator : AbstractValidator<CreatePhoneCode>
{
    public CreatePhoneCodeValidator()
    {
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country code is required.")
            .MaximumLength(5);

        RuleFor(x => x.CountryName)
            .NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(100);
    }
}

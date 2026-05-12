using FluentValidation;

namespace Application.UseCase.PhoneCodes;

public sealed class UpdatePhoneCodeValidator : AbstractValidator<UpdatePhoneCode>
{
    public UpdatePhoneCodeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country code is required.")
            .MaximumLength(5);

        RuleFor(x => x.CountryName)
            .NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(100);
    }
}

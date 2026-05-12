using FluentValidation;

namespace Application.UseCase.PersonPhones;

public sealed class CreatePersonPhoneValidator : AbstractValidator<CreatePersonPhone>
{
    public CreatePersonPhoneValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person id is required.");

        RuleFor(x => x.PhoneCodeId)
            .NotEmpty().WithMessage("Phone code id is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20);
    }
}

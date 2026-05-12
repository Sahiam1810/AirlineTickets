using FluentValidation;

namespace Application.UseCase.PersonPhones;

public sealed class UpdatePersonPhoneValidator : AbstractValidator<UpdatePersonPhone>
{
    public UpdatePersonPhoneValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.PhoneCodeId)
            .NotEmpty().WithMessage("Phone code id is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20);
    }
}

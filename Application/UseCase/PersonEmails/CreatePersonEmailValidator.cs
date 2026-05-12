using FluentValidation;

namespace Application.UseCase.PersonEmails;

public sealed class CreatePersonEmailValidator : AbstractValidator<CreatePersonEmail>
{
    public CreatePersonEmailValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person id is required.");

        RuleFor(x => x.EmailUser)
            .NotEmpty().WithMessage("Email user is required.")
            .MaximumLength(100);

        RuleFor(x => x.EmailDomainId)
            .NotEmpty().WithMessage("Email domain id is required.");
    }
}

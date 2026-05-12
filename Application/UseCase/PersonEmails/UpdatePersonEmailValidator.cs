using FluentValidation;

namespace Application.UseCase.PersonEmails;

public sealed class UpdatePersonEmailValidator : AbstractValidator<UpdatePersonEmail>
{
    public UpdatePersonEmailValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.EmailUser)
            .NotEmpty().WithMessage("Email user is required.")
            .MaximumLength(100);

        RuleFor(x => x.EmailDomainId)
            .NotEmpty().WithMessage("Email domain id is required.");
    }
}

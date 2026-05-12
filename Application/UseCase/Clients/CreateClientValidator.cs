using FluentValidation;

namespace Application.UseCase.Clients;

public sealed class CreateClientValidator : AbstractValidator<CreateClient>
{
    public CreateClientValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person id is required.");
    }
}

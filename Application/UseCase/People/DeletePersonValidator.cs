using FluentValidation;

namespace Application.UseCase.People;

public sealed class DeletePersonValidator : AbstractValidator<DeletePerson>
{
    public DeletePersonValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");
    }
}

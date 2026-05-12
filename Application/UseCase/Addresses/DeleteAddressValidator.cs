using FluentValidation;

namespace Application.UseCase.Addresses;

public sealed class DeleteAddressValidator : AbstractValidator<DeleteAddress>
{
    public DeleteAddressValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");
    }
}

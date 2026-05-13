using FluentValidation;

namespace Application.UseCase.Passengers;

public sealed class UpdatePassengerValidator : AbstractValidator<UpdatePassenger>
{
    public UpdatePassengerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
        RuleFor(x => x.PassengerTypeId).GreaterThan(0);
    }
}

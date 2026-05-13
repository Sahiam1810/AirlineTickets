using FluentValidation;

namespace Application.UseCase.Passengers;

public sealed class CreatePassengerValidator : AbstractValidator<CreatePassenger>
{
    public CreatePassengerValidator()
    {
        RuleFor(x => x.PersonId).GreaterThan(0);
        RuleFor(x => x.PassengerTypeId).GreaterThan(0);
    }
}

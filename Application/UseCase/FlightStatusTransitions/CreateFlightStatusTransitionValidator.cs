using FluentValidation;

namespace Application.UseCase.FlightStatusTransitions;

public sealed class CreateFlightStatusTransitionValidator : AbstractValidator<CreateFlightStatusTransition>
{
    public CreateFlightStatusTransitionValidator()
    {
        RuleFor(x => x.FromStateId)
            .GreaterThan(0).WithMessage("El estado origen es obligatorio.");

        RuleFor(x => x.ToStateId)
            .GreaterThan(0).WithMessage("El estado destino es obligatorio.");

        RuleFor(x => x)
            .Must(x => x.FromStateId != x.ToStateId)
            .WithMessage("El estado origen no puede ser igual al estado destino.");
    }
}

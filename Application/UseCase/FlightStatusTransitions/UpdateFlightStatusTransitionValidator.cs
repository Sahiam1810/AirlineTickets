using FluentValidation;

namespace Application.UseCase.FlightStatusTransitions;

public sealed class UpdateFlightStatusTransitionValidator : AbstractValidator<UpdateFlightStatusTransition>
{
    public UpdateFlightStatusTransitionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.FromStateId)
            .GreaterThan(0).WithMessage("El estado origen es obligatorio.");

        RuleFor(x => x.ToStateId)
            .GreaterThan(0).WithMessage("El estado destino es obligatorio.");

        RuleFor(x => x)
            .Must(x => x.FromStateId != x.ToStateId)
            .WithMessage("El estado origen no puede ser igual al estado destino.");
    }
}

using FluentValidation;

namespace Application.UseCase.ReservationStatusTransitions;

public sealed class CreateReservationStatusTransitionValidator : AbstractValidator<CreateReservationStatusTransition>
{
    public CreateReservationStatusTransitionValidator()
    {
        RuleFor(x => x.FromStatusId).GreaterThan(0);
        RuleFor(x => x.ToStatusId).GreaterThan(0);
        RuleFor(x => x)
            .Must(x => x.FromStatusId != x.ToStatusId)
            .WithMessage("El estado origen y el estado destino deben ser diferentes.");
    }
}

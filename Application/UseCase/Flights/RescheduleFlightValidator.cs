using FluentValidation;

namespace Application.UseCase.Flights;

public sealed class RescheduleFlightValidator : AbstractValidator<RescheduleFlight>
{
    public RescheduleFlightValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
        RuleFor(x => x)
            .Must(x => x.DepartureDate < x.EstimatedArrivalDate)
            .WithMessage("La fecha de salida debe ser menor que la fecha estimada de llegada.");
    }
}

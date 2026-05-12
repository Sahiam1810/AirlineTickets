using FluentValidation;

namespace Application.UseCase.RouteStops;

public sealed class CreateRouteStopValidator : AbstractValidator<CreateRouteStop>
{
    public CreateRouteStopValidator()
    {
        RuleFor(x => x.RouteId)
            .NotEmpty().WithMessage("Route id is required.");

        RuleFor(x => x.StopAirportId)
            .NotEmpty().WithMessage("Stop airport id is required.");

        RuleFor(x => x.Order)
            .GreaterThan(0).WithMessage("Order must be greater than 0.");

        RuleFor(x => x.StopDurationMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("Stop duration must be greater than or equal to 0.");
    }
}

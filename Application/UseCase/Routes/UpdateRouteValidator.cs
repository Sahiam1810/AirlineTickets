using FluentValidation;

namespace Application.UseCase.Routes;

public sealed class UpdateRouteValidator : AbstractValidator<UpdateRoute>
{
    public UpdateRouteValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.OriginAirportId)
            .NotEmpty().WithMessage("Origin airport id is required.");

        RuleFor(x => x.DestinationAirportId)
            .NotEmpty().WithMessage("Destination airport id is required.")
            .NotEqual(x => x.OriginAirportId).WithMessage("Origin airport and destination airport cannot be the same.");

        RuleFor(x => x.DistanceKm)
            .GreaterThan(0).When(x => x.DistanceKm.HasValue);

        RuleFor(x => x.EstimatedDurationMinutes)
            .GreaterThan(0).When(x => x.EstimatedDurationMinutes.HasValue);
    }
}

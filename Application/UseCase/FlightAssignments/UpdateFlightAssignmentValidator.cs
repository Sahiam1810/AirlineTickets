using FluentValidation;

namespace Application.UseCase.FlightAssignments;

public sealed class UpdateFlightAssignmentValidator : AbstractValidator<UpdateFlightAssignment>
{
    public UpdateFlightAssignmentValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
        RuleFor(x => x.FlightId).GreaterThan(0);
        RuleFor(x => x.StaffId).GreaterThan(0);
        RuleFor(x => x.FlightRoleId).GreaterThan(0);
    }
}

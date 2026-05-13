using FluentValidation;

namespace Application.UseCase.FlightAssignments;

public sealed class CreateFlightAssignmentValidator : AbstractValidator<CreateFlightAssignment>
{
    public CreateFlightAssignmentValidator()
    {
        RuleFor(x => x.FlightId).GreaterThan(0);
        RuleFor(x => x.StaffId).GreaterThan(0);
        RuleFor(x => x.FlightRoleId).GreaterThan(0);
    }
}

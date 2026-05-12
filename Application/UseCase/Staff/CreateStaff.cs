using MediatR;

namespace Application.UseCase.Staff;

public sealed record CreateStaff(
    int PersonId,
    int StaffRoleId,
    int? AirlineId,
    int? AirportId,
    DateOnly HireDate,
    bool IsActive) : IRequest<int>;

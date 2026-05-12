using MediatR;

namespace Application.UseCase.Staff;

public sealed record UpdateStaff(
    int Id,
    int StaffRoleId,
    int? AirlineId,
    int? AirportId,
    DateOnly HireDate,
    bool IsActive) : IRequest;

using MediatR;

namespace Application.UseCase.StaffAvailabilities;

public sealed record CreateStaffAvailability(
    int StaffId,
    int AvailabilityStatusId,
    DateTime StartDate,
    DateTime EndDate,
    string? Notes) : IRequest<int>;

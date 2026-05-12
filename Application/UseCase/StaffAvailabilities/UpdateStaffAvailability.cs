using MediatR;

namespace Application.UseCase.StaffAvailabilities;

public sealed record UpdateStaffAvailability(
    int Id,
    int AvailabilityStatusId,
    DateTime StartDate,
    DateTime EndDate,
    string? Notes) : IRequest;

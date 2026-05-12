using MediatR;

namespace Application.UseCase.StaffAvailabilities;

public sealed record DeleteStaffAvailability(int Id) : IRequest;

using MediatR;

namespace Application.UseCase.StaffRoles;

public sealed record UpdateStaffRole(int Id, string Name) : IRequest;

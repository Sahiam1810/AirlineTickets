using MediatR;

namespace Application.UseCase.StaffRoles;

public sealed record CreateStaffRole(string Name) : IRequest<int>;

using MediatR;

namespace Application.UseCase.StaffRoles;

public sealed record DeleteStaffRole(int Id) : IRequest;

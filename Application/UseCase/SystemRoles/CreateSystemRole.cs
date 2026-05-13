using MediatR;

namespace Application.UseCase.SystemRoles;

public sealed record CreateSystemRole(string Name, string? Description) : IRequest<int>;

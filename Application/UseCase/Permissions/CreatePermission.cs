using MediatR;

namespace Application.UseCase.Permissions;

public sealed record CreatePermission(string Name, string? Description) : IRequest<int>;

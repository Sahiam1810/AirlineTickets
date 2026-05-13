namespace Api.Dtos.SystemRoles;

public sealed class CreateSystemRoleRequest
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}

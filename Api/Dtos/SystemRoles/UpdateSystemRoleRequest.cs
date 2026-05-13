namespace Api.Dtos.SystemRoles;

public sealed class UpdateSystemRoleRequest
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}

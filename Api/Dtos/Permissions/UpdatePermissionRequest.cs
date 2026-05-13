namespace Api.Dtos.Permissions;

public sealed class UpdatePermissionRequest
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}

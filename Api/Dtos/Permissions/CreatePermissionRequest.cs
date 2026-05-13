namespace Api.Dtos.Permissions;

public sealed class CreatePermissionRequest
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}

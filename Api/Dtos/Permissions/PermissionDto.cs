namespace Api.Dtos.Permissions;

public sealed class PermissionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

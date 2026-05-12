namespace Api.Dtos.DocumentTypes;

public sealed class DocumentTypeDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
}

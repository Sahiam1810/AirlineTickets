namespace Api.Dtos.DocumentTypes;

public sealed class UpdateDocumentTypeRequest
{
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
}

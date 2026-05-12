namespace Api.Dtos.DocumentTypes;

public sealed class CreateDocumentTypeRequest
{
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
}

using Api.Dtos.DocumentTypes;
using Application.UseCase.DocumentTypes;
using Domain.Entities.People;
using Mapster;

namespace Api.Mappings;

public sealed class DocumentTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DocumentType, DocumentTypeDto>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Code, src => src.Code.Value);

        config.NewConfig<CreateDocumentTypeRequest, CreateDocumentType>();
        config.NewConfig<UpdateDocumentTypeRequest, UpdateDocumentType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

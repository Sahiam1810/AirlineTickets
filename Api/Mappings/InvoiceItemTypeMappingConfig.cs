using Api.Dtos.InvoiceItemTypes;
using Application.UseCase.InvoiceItemTypes;
using Domain.Entities.Payments;
using Mapster;

namespace Api.Mappings;

public sealed class InvoiceItemTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InvoiceItemType, InvoiceItemTypeDto>()
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateInvoiceItemTypeRequest, CreateInvoiceItemType>();
        
        config.NewConfig<UpdateInvoiceItemTypeRequest, UpdateInvoiceItemType>()
            .Map(dest => dest.Id, _ => 0);
    }
}

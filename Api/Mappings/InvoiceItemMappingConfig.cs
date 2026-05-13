using Api.Dtos.InvoiceItems;
using Application.UseCase.InvoiceItems;
using Domain.Entities.Payments;
using Mapster;

namespace Api.Mappings;

public sealed class InvoiceItemMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InvoiceItem, InvoiceItemDto>();

        config.NewConfig<CreateInvoiceItemRequest, CreateInvoiceItem>();
        
        config.NewConfig<UpdateInvoiceItemRequest, UpdateInvoiceItem>()
            .Map(dest => dest.Id, _ => 0);
    }
}

using Api.Dtos.Invoices;
using Application.UseCase.Invoices;
using Domain.Entities.Payments;
using Mapster;

namespace Api.Mappings;

public sealed class InvoiceMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Invoice, InvoiceDto>();
        config.NewConfig<CreateInvoiceRequest, CreateInvoice>();
        config.NewConfig<UpdateInvoiceRequest, UpdateInvoice>()
            .Map(dest => dest.Id, _ => 0);
    }
}

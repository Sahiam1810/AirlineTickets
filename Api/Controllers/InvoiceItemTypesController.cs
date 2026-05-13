using Api.Dtos.InvoiceItemTypes;
using Application.Abstractions;
using Application.UseCase.InvoiceItemTypes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers;

public sealed class InvoiceItemTypesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public InvoiceItemTypesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<InvoiceItemTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<InvoiceItemTypeDto>>> GetAll(CancellationToken ct)
    {
        var invoiceItemTypes = await _uow.InvoiceItemTypes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<InvoiceItemTypeDto>>(invoiceItemTypes);
        return Ok(result);
    }

    [HttpGet("paged")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        var invoiceItemTypes = await _uow.InvoiceItemTypes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.InvoiceItemTypes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<InvoiceItemTypeDto>>(invoiceItemTypes);

        return Ok(new
        {
            page,
            pageSize,
            total,
            items
        });
    }

    [HttpGet("count")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Count([FromQuery] string? search = null, CancellationToken ct = default)
    {
        var total = await _uow.InvoiceItemTypes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InvoiceItemTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoiceItemTypeDto>> GetById(int id, CancellationToken ct)
    {
        var invoiceItemType = await _uow.InvoiceItemTypes.GetByIdAsync(id, ct);
        if (invoiceItemType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<InvoiceItemTypeDto>(invoiceItemType);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceItemTypeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceItemTypeRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateInvoiceItemType>(request);
        var id = await _sender.Send(command, ct);
        var invoiceItemType = await _uow.InvoiceItemTypes.GetByIdAsync(id, ct);
        if (invoiceItemType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<InvoiceItemTypeDto>(invoiceItemType);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceItemTypeRequest request, CancellationToken ct)
    {
        var existing = await _uow.InvoiceItemTypes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateInvoiceItemType>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeleteInvoiceItemType(id);
        try
        {
            await _sender.Send(command, ct);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}

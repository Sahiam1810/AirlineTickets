using Api.Dtos.InvoiceItems;
using Application.Abstractions;
using Application.UseCase.InvoiceItems;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers;

public sealed class InvoiceItemsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public InvoiceItemsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<InvoiceItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<InvoiceItemDto>>> GetAll(CancellationToken ct)
    {
        var invoiceItems = await _uow.InvoiceItems.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<InvoiceItemDto>>(invoiceItems);
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

        var invoiceItems = await _uow.InvoiceItems.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.InvoiceItems.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<InvoiceItemDto>>(invoiceItems);

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
        var total = await _uow.InvoiceItems.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InvoiceItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoiceItemDto>> GetById(int id, CancellationToken ct)
    {
        var invoiceItem = await _uow.InvoiceItems.GetByIdAsync(id, ct);
        if (invoiceItem is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<InvoiceItemDto>(invoiceItem);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceItemDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceItemRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateInvoiceItem>(request);
        var id = await _sender.Send(command, ct);
        var invoiceItem = await _uow.InvoiceItems.GetByIdAsync(id, ct);
        if (invoiceItem is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<InvoiceItemDto>(invoiceItem);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceItemRequest request, CancellationToken ct)
    {
        var existing = await _uow.InvoiceItems.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateInvoiceItem>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeleteInvoiceItem(id);
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

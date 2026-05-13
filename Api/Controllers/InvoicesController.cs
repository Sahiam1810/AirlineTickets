using Api.Dtos.Invoices;
using Application.Abstractions;
using Application.UseCase.Invoices;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class InvoicesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public InvoicesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<InvoiceDto>>> GetAll(CancellationToken ct)
    {
        var invoices = await _uow.Invoices.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<InvoiceDto>>(invoices);
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

        var invoices = await _uow.Invoices.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Invoices.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<InvoiceDto>>(invoices);

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
        var total = await _uow.Invoices.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoiceDto>> GetById(int id, CancellationToken ct)
    {
        var invoice = await _uow.Invoices.GetByIdAsync(id, ct);
        if (invoice is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<InvoiceDto>(invoice);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateInvoice>(request);
        var id = await _sender.Send(command, ct);
        var invoice = await _uow.Invoices.GetByIdAsync(id, ct);
        if (invoice is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<InvoiceDto>(invoice);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceRequest request, CancellationToken ct)
    {
        var existing = await _uow.Invoices.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateInvoice>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var invoice = await _uow.Invoices.GetByIdAsync(id, ct);
        if (invoice is null)
        {
            return NotFound();
        }

        var command = new DeleteInvoice(id);
        await _sender.Send(command, ct);

        return NoContent();
    }
}

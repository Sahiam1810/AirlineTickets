using Api.Dtos.TicketStatuses;
using Application.Abstractions;
using Application.UseCase.TicketStatuses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class TicketStatusesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public TicketStatusesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TicketStatusDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TicketStatusDto>>> GetAll(CancellationToken ct)
    {
        var ticketStatuses = await _uow.TicketStatuses.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<TicketStatusDto>>(ticketStatuses);
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

        var ticketStatuses = await _uow.TicketStatuses.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.TicketStatuses.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<TicketStatusDto>>(ticketStatuses);

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
        var total = await _uow.TicketStatuses.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TicketStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketStatusDto>> GetById(int id, CancellationToken ct)
    {
        var ticketStatus = await _uow.TicketStatuses.GetByIdAsync(id, ct);
        if (ticketStatus is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<TicketStatusDto>(ticketStatus);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TicketStatusDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTicketStatusRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateTicketStatus>(request);
        var id = await _sender.Send(command, ct);
        var ticketStatus = await _uow.TicketStatuses.GetByIdAsync(id, ct);
        if (ticketStatus is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<TicketStatusDto>(ticketStatus);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTicketStatusRequest request, CancellationToken ct)
    {
        var existing = await _uow.TicketStatuses.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateTicketStatus>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ticketStatus = await _uow.TicketStatuses.GetByIdAsync(id, ct);
        if (ticketStatus is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteTicketStatus(id), ct);

        return NoContent();
    }
}

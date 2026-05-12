using Api.Dtos.AvailabilityStatuses;
using Application.Abstractions;
using Application.UseCase.AvailabilityStatuses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class AvailabilityStatusesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AvailabilityStatusesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AvailabilityStatusDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AvailabilityStatusDto>>> GetAll(CancellationToken ct)
    {
        var availabilityStatuses = await _uow.AvailabilityStatuses.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<AvailabilityStatusDto>>(availabilityStatuses);
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

        var availabilityStatuses = await _uow.AvailabilityStatuses.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.AvailabilityStatuses.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<AvailabilityStatusDto>>(availabilityStatuses);

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
        var total = await _uow.AvailabilityStatuses.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AvailabilityStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AvailabilityStatusDto>> GetById(int id, CancellationToken ct)
    {
        var availabilityStatus = await _uow.AvailabilityStatuses.GetByIdAsync(id, ct);
        if (availabilityStatus is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AvailabilityStatusDto>(availabilityStatus);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AvailabilityStatusDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAvailabilityStatusRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAvailabilityStatus>(request);
        var id = await _sender.Send(command, ct);
        var availabilityStatus = await _uow.AvailabilityStatuses.GetByIdAsync(id, ct);
        if (availabilityStatus is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AvailabilityStatusDto>(availabilityStatus);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAvailabilityStatusRequest request, CancellationToken ct)
    {
        var existing = await _uow.AvailabilityStatuses.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateAvailabilityStatus>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var availabilityStatus = await _uow.AvailabilityStatuses.GetByIdAsync(id, ct);
        if (availabilityStatus is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteAvailabilityStatus(id), ct);

        return NoContent();
    }
}

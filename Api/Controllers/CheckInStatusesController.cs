using Api.Dtos.CheckInStatuses;
using Application.Abstractions;
using Application.UseCase.CheckInStatuses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class CheckInStatusesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CheckInStatusesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CheckInStatusDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CheckInStatusDto>>> GetAll(CancellationToken ct)
    {
        var checkInStatuses = await _uow.CheckInStatuses.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<CheckInStatusDto>>(checkInStatuses);
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

        var checkInStatuses = await _uow.CheckInStatuses.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.CheckInStatuses.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<CheckInStatusDto>>(checkInStatuses);

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
        var total = await _uow.CheckInStatuses.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CheckInStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckInStatusDto>> GetById(int id, CancellationToken ct)
    {
        var checkInStatus = await _uow.CheckInStatuses.GetByIdAsync(id, ct);
        if (checkInStatus is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CheckInStatusDto>(checkInStatus);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CheckInStatusDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCheckInStatusRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateCheckInStatus>(request);
        var id = await _sender.Send(command, ct);
        var checkInStatus = await _uow.CheckInStatuses.GetByIdAsync(id, ct);
        if (checkInStatus is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CheckInStatusDto>(checkInStatus);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCheckInStatusRequest request, CancellationToken ct)
    {
        var existing = await _uow.CheckInStatuses.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateCheckInStatus>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var checkInStatus = await _uow.CheckInStatuses.GetByIdAsync(id, ct);
        if (checkInStatus is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteCheckInStatus(id), ct);

        return NoContent();
    }
}

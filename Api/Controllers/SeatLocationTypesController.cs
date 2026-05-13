using Api.Dtos.SeatLocationTypes;
using Application.Abstractions;
using Application.UseCase.SeatLocationTypes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class SeatLocationTypesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public SeatLocationTypesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SeatLocationTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SeatLocationTypeDto>>> GetAll(CancellationToken ct)
    {
        var seatLocationTypes = await _uow.SeatLocationTypes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<SeatLocationTypeDto>>(seatLocationTypes);
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

        var seatLocationTypes = await _uow.SeatLocationTypes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.SeatLocationTypes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<SeatLocationTypeDto>>(seatLocationTypes);

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
        var total = await _uow.SeatLocationTypes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SeatLocationTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SeatLocationTypeDto>> GetById(int id, CancellationToken ct)
    {
        var seatLocationType = await _uow.SeatLocationTypes.GetByIdAsync(id, ct);
        if (seatLocationType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<SeatLocationTypeDto>(seatLocationType);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SeatLocationTypeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSeatLocationTypeRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateSeatLocationType>(request);
        var id = await _sender.Send(command, ct);
        var seatLocationType = await _uow.SeatLocationTypes.GetByIdAsync(id, ct);
        if (seatLocationType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<SeatLocationTypeDto>(seatLocationType);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSeatLocationTypeRequest request, CancellationToken ct)
    {
        var existing = await _uow.SeatLocationTypes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateSeatLocationType>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var seatLocationType = await _uow.SeatLocationTypes.GetByIdAsync(id, ct);
        if (seatLocationType is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteSeatLocationType(id), ct);

        return NoContent();
    }
}

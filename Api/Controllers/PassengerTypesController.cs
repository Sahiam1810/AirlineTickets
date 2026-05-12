using Api.Dtos.PassengerTypes;
using Application.Abstractions;
using Application.UseCase.PassengerTypes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PassengerTypesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PassengerTypesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PassengerTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PassengerTypeDto>>> GetAll(CancellationToken ct)
    {
        var passengerTypes = await _uow.PassengerTypes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<PassengerTypeDto>>(passengerTypes);
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

        var passengerTypes = await _uow.PassengerTypes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.PassengerTypes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PassengerTypeDto>>(passengerTypes);

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
        var total = await _uow.PassengerTypes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PassengerTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassengerTypeDto>> GetById(int id, CancellationToken ct)
    {
        var passengerType = await _uow.PassengerTypes.GetByIdAsync(id, ct);
        if (passengerType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PassengerTypeDto>(passengerType);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PassengerTypeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePassengerTypeRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePassengerType>(request);
        var id = await _sender.Send(command, ct);
        var passengerType = await _uow.PassengerTypes.GetByIdAsync(id, ct);
        if (passengerType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PassengerTypeDto>(passengerType);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePassengerTypeRequest request, CancellationToken ct)
    {
        var existing = await _uow.PassengerTypes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePassengerType>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var passengerType = await _uow.PassengerTypes.GetByIdAsync(id, ct);
        if (passengerType is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeletePassengerType(id), ct);

        return NoContent();
    }
}

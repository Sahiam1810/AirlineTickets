using Api.Dtos.Passengers;
using Application.Abstractions;
using Application.UseCase.Passengers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PassengersController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PassengersController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PassengerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PassengerDto>>> GetAll(CancellationToken ct)
    {
        var passengers = await _uow.Passengers.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<PassengerDto>>(passengers);
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

        var passengers = await _uow.Passengers.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Passengers.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PassengerDto>>(passengers);

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
        var total = await _uow.Passengers.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassengerDto>> GetById(int id, CancellationToken ct)
    {
        var passenger = await _uow.Passengers.GetByIdAsync(id, ct);
        if (passenger is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PassengerDto>(passenger);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePassengerRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePassenger>(request);
        var id = await _sender.Send(command, ct);
        var passenger = await _uow.Passengers.GetByIdAsync(id, ct);
        if (passenger is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PassengerDto>(passenger);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePassengerRequest request, CancellationToken ct)
    {
        var existing = await _uow.Passengers.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePassenger>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var passenger = await _uow.Passengers.GetByIdAsync(id, ct);
        if (passenger is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeletePassenger(id), ct);

        return NoContent();
    }
}

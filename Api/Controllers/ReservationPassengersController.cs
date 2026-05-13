using Api.Dtos.ReservationPassengers;
using Application.Abstractions;
using Application.UseCase.ReservationPassengers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class ReservationPassengersController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ReservationPassengersController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReservationPassengerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReservationPassengerDto>>> GetAll(CancellationToken ct)
    {
        var reservationPassengers = await _uow.ReservationPassengers.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<ReservationPassengerDto>>(reservationPassengers);
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

        var reservationPassengers = await _uow.ReservationPassengers.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.ReservationPassengers.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<ReservationPassengerDto>>(reservationPassengers);

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
        var total = await _uow.ReservationPassengers.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReservationPassengerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationPassengerDto>> GetById(int id, CancellationToken ct)
    {
        var reservationPassenger = await _uow.ReservationPassengers.GetByIdAsync(id, ct);
        if (reservationPassenger is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationPassengerDto>(reservationPassenger);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservationPassengerDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateReservationPassengerRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateReservationPassenger>(request);
        var id = await _sender.Send(command, ct);
        var reservationPassenger = await _uow.ReservationPassengers.GetByIdAsync(id, ct);
        if (reservationPassenger is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationPassengerDto>(reservationPassenger);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationPassengerRequest request, CancellationToken ct)
    {
        var existing = await _uow.ReservationPassengers.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateReservationPassenger>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var reservationPassenger = await _uow.ReservationPassengers.GetByIdAsync(id, ct);
        if (reservationPassenger is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteReservationPassenger(id), ct);

        return NoContent();
    }
}

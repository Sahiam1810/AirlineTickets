using Api.Dtos.ReservationFlights;
using Application.Abstractions;
using Application.UseCase.ReservationFlights;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class ReservationFlightsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ReservationFlightsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReservationFlightDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReservationFlightDto>>> GetAll(CancellationToken ct)
    {
        var reservationFlights = await _uow.ReservationFlights.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<ReservationFlightDto>>(reservationFlights);
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

        var reservationFlights = await _uow.ReservationFlights.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.ReservationFlights.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<ReservationFlightDto>>(reservationFlights);

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
        var total = await _uow.ReservationFlights.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReservationFlightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationFlightDto>> GetById(int id, CancellationToken ct)
    {
        var reservationFlight = await _uow.ReservationFlights.GetByIdAsync(id, ct);
        if (reservationFlight is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationFlightDto>(reservationFlight);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservationFlightDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateReservationFlightRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateReservationFlight>(request);
        var id = await _sender.Send(command, ct);
        var reservationFlight = await _uow.ReservationFlights.GetByIdAsync(id, ct);
        if (reservationFlight is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationFlightDto>(reservationFlight);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationFlightRequest request, CancellationToken ct)
    {
        var existing = await _uow.ReservationFlights.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateReservationFlight>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var reservationFlight = await _uow.ReservationFlights.GetByIdAsync(id, ct);
        if (reservationFlight is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteReservationFlight(id), ct);

        return NoContent();
    }
}

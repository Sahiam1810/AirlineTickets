using Api.Dtos.StaffAvailabilities;
using Application.Abstractions;
using Application.UseCase.StaffAvailabilities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class StaffAvailabilitiesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public StaffAvailabilitiesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StaffAvailabilityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StaffAvailabilityDto>>> GetAll(CancellationToken ct)
    {
        var staffAvailabilities = await _uow.StaffAvailabilities.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<StaffAvailabilityDto>>(staffAvailabilities);
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

        var staffAvailabilities = await _uow.StaffAvailabilities.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.StaffAvailabilities.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<StaffAvailabilityDto>>(staffAvailabilities);

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
        var total = await _uow.StaffAvailabilities.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StaffAvailabilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StaffAvailabilityDto>> GetById(int id, CancellationToken ct)
    {
        var staffAvailability = await _uow.StaffAvailabilities.GetByIdAsync(id, ct);
        if (staffAvailability is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<StaffAvailabilityDto>(staffAvailability);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StaffAvailabilityDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateStaffAvailabilityRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateStaffAvailability>(request);
        var id = await _sender.Send(command, ct);
        var staffAvailability = await _uow.StaffAvailabilities.GetByIdAsync(id, ct);
        if (staffAvailability is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<StaffAvailabilityDto>(staffAvailability);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStaffAvailabilityRequest request, CancellationToken ct)
    {
        var existing = await _uow.StaffAvailabilities.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateStaffAvailability>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var staffAvailability = await _uow.StaffAvailabilities.GetByIdAsync(id, ct);
        if (staffAvailability is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteStaffAvailability(id), ct);

        return NoContent();
    }
}

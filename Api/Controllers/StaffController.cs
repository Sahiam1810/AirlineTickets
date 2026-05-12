using Api.Dtos.Staff;
using Application.Abstractions;
using Application.UseCase.Staff;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class StaffController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public StaffController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StaffDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StaffDto>>> GetAll(CancellationToken ct)
    {
        var staff = await _uow.Staff.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<StaffDto>>(staff);
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

        var staff = await _uow.Staff.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Staff.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<StaffDto>>(staff);

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
        var total = await _uow.Staff.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StaffDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StaffDto>> GetById(int id, CancellationToken ct)
    {
        var staff = await _uow.Staff.GetByIdAsync(id, ct);
        if (staff is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<StaffDto>(staff);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StaffDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateStaffRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateStaff>(request);
        var id = await _sender.Send(command, ct);
        var staff = await _uow.Staff.GetByIdAsync(id, ct);
        if (staff is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<StaffDto>(staff);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStaffRequest request, CancellationToken ct)
    {
        var existing = await _uow.Staff.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateStaff>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var staff = await _uow.Staff.GetByIdAsync(id, ct);
        if (staff is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteStaff(id), ct);

        return NoContent();
    }
}

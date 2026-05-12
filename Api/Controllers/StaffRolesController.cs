using Api.Dtos.StaffRoles;
using Application.Abstractions;
using Application.UseCase.StaffRoles;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class StaffRolesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public StaffRolesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StaffRoleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StaffRoleDto>>> GetAll(CancellationToken ct)
    {
        var staffRoles = await _uow.StaffRoles.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<StaffRoleDto>>(staffRoles);
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

        var staffRoles = await _uow.StaffRoles.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.StaffRoles.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<StaffRoleDto>>(staffRoles);

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
        var total = await _uow.StaffRoles.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StaffRoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StaffRoleDto>> GetById(int id, CancellationToken ct)
    {
        var staffRole = await _uow.StaffRoles.GetByIdAsync(id, ct);
        if (staffRole is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<StaffRoleDto>(staffRole);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StaffRoleDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateStaffRoleRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateStaffRole>(request);
        var id = await _sender.Send(command, ct);
        var staffRole = await _uow.StaffRoles.GetByIdAsync(id, ct);
        if (staffRole is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<StaffRoleDto>(staffRole);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStaffRoleRequest request, CancellationToken ct)
    {
        var existing = await _uow.StaffRoles.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateStaffRole>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var staffRole = await _uow.StaffRoles.GetByIdAsync(id, ct);
        if (staffRole is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteStaffRole(id), ct);

        return NoContent();
    }
}

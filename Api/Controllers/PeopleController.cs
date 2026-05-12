using Api.Dtos.People;
using Application.Abstractions;
using Application.UseCase.People;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PeopleController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PeopleController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PersonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PersonDto>>> GetAll(CancellationToken ct)
    {
        var people = await _uow.People.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<PersonDto>>(people);
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

        var people = await _uow.People.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.People.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PersonDto>>(people);

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
        var total = await _uow.People.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDto>> GetById(int id, CancellationToken ct)
    {
        var person = await _uow.People.GetByIdAsync(id, ct);
        if (person is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PersonDto>(person);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePersonRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePerson>(request);
        var id = await _sender.Send(command, ct);
        var person = await _uow.People.GetByIdAsync(id, ct);
        if (person is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PersonDto>(person);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonRequest request, CancellationToken ct)
    {
        var existing = await _uow.People.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePerson>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var person = await _uow.People.GetByIdAsync(id, ct);
        if (person is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeletePerson(id), ct);

        return NoContent();
    }
}

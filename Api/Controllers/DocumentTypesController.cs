using Api.Dtos.DocumentTypes;
using Application.Abstractions;
using Application.UseCase.DocumentTypes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class DocumentTypesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public DocumentTypesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DocumentTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DocumentTypeDto>>> GetAll(CancellationToken ct)
    {
        var documentTypes = await _uow.DocumentTypes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<DocumentTypeDto>>(documentTypes);
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

        var documentTypes = await _uow.DocumentTypes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.DocumentTypes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<DocumentTypeDto>>(documentTypes);

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
        var total = await _uow.DocumentTypes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DocumentTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DocumentTypeDto>> GetById(int id, CancellationToken ct)
    {
        var documentType = await _uow.DocumentTypes.GetByIdAsync(id, ct);
        if (documentType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<DocumentTypeDto>(documentType);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DocumentTypeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDocumentTypeRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateDocumentType>(request);
        var id = await _sender.Send(command, ct);
        var documentType = await _uow.DocumentTypes.GetByIdAsync(id, ct);
        if (documentType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<DocumentTypeDto>(documentType);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentTypeRequest request, CancellationToken ct)
    {
        var existing = await _uow.DocumentTypes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateDocumentType>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var documentType = await _uow.DocumentTypes.GetByIdAsync(id, ct);
        if (documentType is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteDocumentType(id), ct);

        return NoContent();
    }
}

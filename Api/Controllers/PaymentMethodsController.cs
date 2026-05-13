using Api.Dtos.PaymentMethods;
using Application.Abstractions;
using Application.UseCase.PaymentMethods;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PaymentMethodsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PaymentMethodsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentMethodDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PaymentMethodDto>>> GetAll(CancellationToken ct)
    {
        var paymentMethods = await _uow.PaymentMethods.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<PaymentMethodDto>>(paymentMethods);
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

        var paymentMethods = await _uow.PaymentMethods.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.PaymentMethods.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PaymentMethodDto>>(paymentMethods);

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
        var total = await _uow.PaymentMethods.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PaymentMethodDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentMethodDto>> GetById(int id, CancellationToken ct)
    {
        var paymentMethod = await _uow.PaymentMethods.GetByIdAsync(id, ct);
        if (paymentMethod is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PaymentMethodDto>(paymentMethod);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaymentMethodDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePaymentMethod>(request);
        var id = await _sender.Send(command, ct);
        var paymentMethod = await _uow.PaymentMethods.GetByIdAsync(id, ct);
        if (paymentMethod is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PaymentMethodDto>(paymentMethod);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentMethodRequest request, CancellationToken ct)
    {
        var existing = await _uow.PaymentMethods.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePaymentMethod>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeletePaymentMethod(id);
        try
        {
            await _sender.Send(command, ct);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}

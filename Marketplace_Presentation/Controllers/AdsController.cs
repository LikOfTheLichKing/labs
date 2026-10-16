using Marketplace.Application.Dtos;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdsController : ControllerBase
{
    private readonly IAdService _adService;

    public AdsController(IAdService adService)
    {
        _adService = adService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAdDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adService.CreateAdAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adService.GetAdByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> SubmitForModeration(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adService.SubmitForModerationAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/moderate")]
    public async Task<IActionResult> Moderate(Guid id, [FromQuery] bool isApproved, [FromQuery] string? rejectionReason, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adService.ModerateAdAsync(id, isApproved, rejectionReason, cancellationToken);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}